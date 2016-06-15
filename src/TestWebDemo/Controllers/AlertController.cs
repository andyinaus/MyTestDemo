using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Raven.Client;
using TestWebDemo.Domain;
using TestWebDemo.Models;
using TestWebDemo.Services;

namespace TestWebDemo.Controllers
{
    [AllowAnonymous]
    public class AlertController : Controller
    {
        private readonly IAsyncDocumentSession _asyncDocumentSession;
        private readonly IAggregateRootFactory _aggregateRootFactory;

        public AlertController(IAsyncDocumentSession asyncDocumentSession, IAggregateRootFactory aggregateRootFactory)
        {
            _asyncDocumentSession = asyncDocumentSession;
            _aggregateRootFactory = aggregateRootFactory;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var alerts = await _asyncDocumentSession.Query<Alert>().ToListAsync();

            var model = alerts.Select(alert => new AlertViewModel(alert));

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new AlertViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(AlertViewModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            try
            {
                var alert = await _aggregateRootFactory.CreateAlert();

                alert.SetTitle(model.Title);
                alert.SetContent(model.Content);

                await _asyncDocumentSession.StoreAsync(alert);
                await _asyncDocumentSession.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Debug.Write($"An unexpected error occured - {exception.Message}");
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}