using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Product.WebApi.Acceptance.Tests;

public class ProductGetRoutesTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>().WithStubbedBearer();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestCase("/product/languages")]
    [TestCase("/product/markets")]
    [TestCase("/product/families")]
    [TestCase("/product/tags")]
    [TestCase("/product/products")]
    [TestCase("/product/items")]
    [TestCase("/product/assets")]
    [TestCase("/product/product-tags")]
    [TestCase("/product/product-markets")]
    [TestCase("/product/asset-markets")]
    public async Task List_routes_return_ok(string path)
    {
        var response = await _client.GetAsync(path);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Missing_product_returns_not_found()
    {
        var response = await _client.GetAsync($"/product/products/{Guid.NewGuid()}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Unknown_country_filter_returns_bad_request()
    {
        var response = await _client.GetAsync("/product/products?country=xx");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}

/// <summary>
/// Runs against the real JWT bearer scheme: no stub, no live IDP call needed because
/// the handler rejects missing and malformed tokens before fetching IDP metadata.
/// </summary>
public class ProductApiAuthorizationTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestCase("/product/languages")]
    [TestCase("/product/products")]
    [TestCase("/product/assets")]
    public async Task Product_data_is_not_served_anonymously(string path)
    {
        var response = await _client.GetAsync(path);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Malformed_bearer_token_is_rejected()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "not-a-real-token");

        var response = await _client.GetAsync("/product/languages");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Static_demo_page_is_still_anonymous()
    {
        var response = await _client.GetAsync("/");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Swagger_ui_is_anonymous()
    {
        var response = await _client.GetAsync("/swagger/index.html");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Openapi_document_is_anonymous_and_describes_the_product_api()
    {
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var json = await response.Content.ReadAsStringAsync();
        Assert.That(json, Does.Contain("/product/languages"));
        Assert.That(json, Does.Contain("\"Bearer\""));
        Assert.That(json, Does.Not.Contain("\"/api/languages\""));
        Assert.That(json, Does.Not.Contain("\"/bff/"));
    }

    [Test]
    public async Task Openapi_bff_document_describes_the_portal_api()
    {
        var response = await _client.GetAsync("/swagger/bff/swagger.json");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var json = await response.Content.ReadAsStringAsync();
        Assert.That(json, Does.Contain("/api/languages"));
        Assert.That(json, Does.Contain("X-CSRF"));
        Assert.That(json, Does.Not.Contain("\"/product/languages\""));
    }
}

public class ProductApiScopeTests
{
    private WebApplicationFactory<Program> _factory = null!;

    [SetUp]
    public void SetUp() => _factory = new WebApplicationFactory<Program>();

    [TearDown]
    public void TearDown() => _factory.Dispose();

    [Test]
    public async Task Authenticated_caller_without_the_read_scope_is_forbidden()
    {
        using var factory = _factory.WithScopelessBearer();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/product/languages");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}
