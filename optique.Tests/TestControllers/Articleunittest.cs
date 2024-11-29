using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Controllers;
using optique.IServices;
using optique.Dtos;
using optique.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ArticleMvcControllerTests
{
    private readonly Mock<IArticleService> _mockArticleService;
    private readonly Mock<IFournisseurService> _mockFournisseurService;
    private readonly Mock<IRefMarqueService> _mockMarqueService;
    private readonly Mock<IRefTypeService> _mockTypeService;
    private readonly Mock<ILogger<ArticleMvcController>> _mockLogger;
    private readonly ArticleMvcController _controller;

    public ArticleMvcControllerTests()
    {
        _mockArticleService = new Mock<IArticleService>();
        _mockFournisseurService = new Mock<IFournisseurService>();
        _mockMarqueService = new Mock<IRefMarqueService>();
        _mockTypeService = new Mock<IRefTypeService>();
        _mockLogger = new Mock<ILogger<ArticleMvcController>>();

        _controller = new ArticleMvcController(
            _mockArticleService.Object,
            _mockFournisseurService.Object,
            _mockMarqueService.Object,
            _mockTypeService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Index_Returns_ViewResult_With_ArticleViewModel()
    {
        // Arrange
        var fournisseurs = new List<FournisseurDTO> 
        { 
            new FournisseurDTO 
            {
                Id = 1, 
                NomFournisseur = "Fournisseur 1", 
                Adresse = "123 rue Example", 
                Telephone = "0123456789", 
                ICE = "ICE123456", 
                Pays = "France", 
                DeviseLibelle = "EUR", 
                DeviseCode = "EUR"
            } 
        };

        var marques = new List<RefMarqueDTO> 
        { 
            new RefMarqueDTO 
            { 
                Id = 1, 
                Libelle = "Marque 1", 
                Code = "M001" 
            } 
        };

        var types = new List<RefTypeDTO> 
        { 
            new RefTypeDTO 
            { 
                Id = 1, 
                Libelle = "Type 1", 
                Code = "T001" 
            } 
        };

        var articles = new List<ArticleDTO> 
        { 
            new ArticleDTO { Id = 1, Description = "Article 1", Reference = "Ref1" }
        };

        _mockFournisseurService.Setup(s => s.GetAllAsync()).ReturnsAsync(fournisseurs);
        _mockMarqueService.Setup(s => s.GetAllAsync()).ReturnsAsync(marques);
        _mockTypeService.Setup(s => s.GetAllAsync()).ReturnsAsync(types);
        _mockArticleService.Setup(s => s.GetAllAsync()).ReturnsAsync(articles);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ArticleViewModel>(viewResult.Model);
        Assert.Single(model.Articles);  // Utilisation de Assert.Single
    }

    [Fact]
    public async Task Filter_Returns_Correct_View_With_Filtered_Articles()
    {
        // Arrange
        var filteredArticles = new List<ArticleDTO> 
        { 
            new ArticleDTO { Id = 1, Description = "Filtered Article 1", Reference = "Ref1" }
        };

        _mockArticleService.Setup(s => s.SearchArticlesByCriteriaAsync("Fournisseur 1", "Marque 1", "Type 1", "Ref1"))
                           .ReturnsAsync(filteredArticles);

        _mockFournisseurService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<FournisseurDTO>());
        _mockMarqueService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<RefMarqueDTO>());
        _mockTypeService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<RefTypeDTO>());

        // Act
        var result = await _controller.Filter("Fournisseur 1", "Marque 1", "Type 1", "Ref1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ArticleViewModel>(viewResult.Model);
        Assert.Single(model.Articles);
    }

    [Fact]
    public async Task Create_Returns_View_With_New_ArticleDTO()
    {
        // Arrange
        _mockFournisseurService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<FournisseurDTO>());
        _mockMarqueService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<RefMarqueDTO>());
        _mockTypeService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<RefTypeDTO>());

        // Act
        var result = await _controller.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ArticleDTO>(viewResult.Model);
        Assert.Equal("default description", model.Description);
    }

    [Fact]
    public async Task Create_Post_Redirects_To_Index_On_Success()
    {
        // Arrange
        var article = new ArticleDTO { Id = 1, Description = "New Article", Reference = "NewRef" };

        // Act
        var result = await _controller.Create(article);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockArticleService.Verify(s => s.AddAsync(It.IsAny<ArticleDTO>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Redirects_To_Index_After_Deleting_Article()
    {
        // Arrange
        var articleId = 1;

        // Act
        var result = await _controller.Delete(articleId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockArticleService.Verify(s => s.DeleteAsync(articleId), Times.Once);
    }
}
