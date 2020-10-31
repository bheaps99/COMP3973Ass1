using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HarryPotter.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace HarryPotter.Controllers
{
    public class BooksController : Controller
    {
        const string COL_BASE_URL = "https://www.googleapis.com/books/v1/volumes?q=harry+potter";
        
        const string DET_BASE_URL = "https://www.googleapis.com/books/v1/volumes/";
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _clientFactory;

        public bool GetBooksError { get; private set; }

        public BooksController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        private async Task<Container> GetBooks()
        {
            Container MyBooks = new Container();

            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{COL_BASE_URL}");
            message.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                
                MyBooks = await System.Text.Json.JsonSerializer.DeserializeAsync<Container>(responseStream);
            }
            else
            {
                GetBooksError = true;
                MyBooks = new Container();
            }

            return MyBooks;
        }

        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            Container container = await GetBooks();
            var books = new List<Tuple<string,string>>();  // id  title

            foreach(Book book in container.items)
            {
                books.Add(Tuple.Create(book.id, book.volumeInfo.title));
            }
            ViewBag.Heading = "Book Title";
            return View(books);
        }

        private async Task<BookDetails> GetBook(string id)
        {
            BookDetails Book = new BookDetails();

            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{DET_BASE_URL}"+id);
            message.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                
                Book = await System.Text.Json.JsonSerializer.DeserializeAsync<BookDetails>(responseStream);
            }
            else
            {
                GetBooksError = true;
                Book = new BookDetails();
            }

            return Book;
        }


        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            BookDetails book = await GetBook(id);

            book.volumeInfo.description = Regex.Replace(book.volumeInfo.description, "<.*?>", string.Empty);

            return View(book);
        }
    }
}
