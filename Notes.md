# Steps to Create an ASP.NET Core MVC app with MySQL Database:
1. Create project directory. Be sure to follow naming conventions. ie: `$ mkdir TodoList.Solution`
2. Navigate into project directory and initialize git 
    - `$ cd ToDoList.Solution`
    - `$ git init`
3. Create Project subdirectory
    - `$ mkdir TodoList`
4. Create `.gitignore` and add list of directories and files git should ignore.
    - `$ touch .gitignore`
      <details><summary><code>ToDoList.Solution/.gitignore</code></summary> 

      ```
      bin
      obj
      appsettings.json
      ```
      </details>
5. Make first commit for `.gitignore` so that Git doesn't track unwanted files.
    - `$ git add .gitignore` 
    - `$ git commit -m "add .gitignore"` 
6. Navigate to project directory
    - `$ cd TodoList` 
7. Create required directories: 
    - `$ mkdir Controllers Models Properties Views wwwroot`
8. Create configuration files: 
    - `$ touch Program.cs ToDoList.csproj appsettings.json Properties/launchSettings.json`
9. Add required content to these files.

      <details><summary><code>ToDoList/ToDoList.csproj</code></summary> 

      ```c#
      <Project Sdk="Microsoft.NET.Sdk.Web">

        <PropertyGroup>
          <TargetFramework>net6.0</TargetFramework>
        </PropertyGroup>

        <ItemGroup>
          <PackageReference Include="Microsoft.EntityFrameworkCore" Version="6.0.0" />
          <PackageReference Include="Pomelo.EntityFrameworkCore.MySql"Version="6.0.0" />
        </ItemGroup>

      </Project>
      ```
      </details>
      
      <details><summary><code>ToDoList/Program.cs</code></summary> 

      ```c#
      using Microsoft.AspNetCore.Builder;
      using Microsoft.EntityFrameworkCore;
      using Microsoft.Extensions.DependencyInjection;
      using ToDoList.Models;
      // be sure to change the namespace to match your project
      namespace ToDoList
      {
        class Program
        {
          static void Main(string[] args)
          {
          
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            // be sure to update the line below for your project
            builder.Services.AddDbContext<ToDoListContext>(
                              dbContextOptions => dbContextOptions
                                .UseMySql(
                                  builder.Configuration     ["ConnectionStrings:DefaultConnection"],      ServerVersion.AutoDetect(builder.   Configuration    ["ConnectionStrings:DefaultConnection"]
                                )
                              )
                            );

            WebApplication app = builder.Build();

            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
          }
        }
      }
      ```
      </details>

      <details><summary><code>ToDoList/appsettings.json</code></summary> 

      ```json
      {
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost;Port=3306;database=[YOUR-DATABASE-NAME];uid=root;pwd=[YOUR-MySQL-PASSWORD];"
          }
      }
      ```
      </details>

      <details><summary><code>ToDoList/Properties/launchSettings.json</code></summary> 

      ```json
      {
          "profiles": {
            "development": {
            "commandName": "Project",
            "dotnetRunMessages": true,
            "launchBrowser": true,
            "applicationUrl": "https://localhost:5001;http://localhost:5000",
            "environmentVariables": {
              "ASPNETCORE_ENVIRONMENT": "Development"
            }
          },
          "production": {
             "commandName": "Project",
             "dotnetRunMessages": true,
             "launchBrowser": true,
             "applicationUrl": "https://localhost:5001;http://localhost:5000",
             "environmentVariables": {
               "ASPNETCORE_ENVIRONMENT": "Production"
             }
           }
         }
      }
      ```
      </details>

10. Optional but its a good idea to test your configuration at this point with: 
    - `$ dotnet build`
11. Build Models
    -  Model Naming Conventions for EF Core:
        - Column names in DB must match property names of Models in the app. These are case-sensitive.
        - For a property to be recognized as a primary key, we need to name the property `Id` or `[ClassName]Id`.
    - Example from ToDoList:
      <details><summary><code>ToDoList/Models/Item.cs</code></summary> 

      ```c#
      namespace ToDoList.Models
      {
        public class Item
        {
          // Property names must match DB column names exactly.
          // Be sure to use either Id or [ClassNameId]
          public int ItemId { get; set; }
          public string Description { get; set; }
        }
      }
      ```
      </details>

        <details><summary><code>ToDoList/Models/Category.cs</code></summary> 

      ```c#
      namespace ToDoList.Models
      {
        public class Category
        {
          public int CategoryId { get; set; }
          public string Name { get; set; }
        }
      }
      ```
      </details>

12. Create Database Context and Entities for the Models.
    <details><summary><code>ToDoList/Models/ToDoListContext.cs</code></summary> 

    ```c#
    using Microsoft.EntityFrameworkCore;

    namespace ToDoList.Models
    {
      public class ToDoListContext : DbContext
      {
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories {get; set;}
        
        public ToDoListContext(DbContextOptions     options) : base(options) { }
      }
    }
    ```
    </details>

13. Build Controllers and Views.

<hr />

## CREATING A ONE-TO-MANY RELATIONSHIP BETWEEN TWO MODELS.
1. Decide which Model is the "One" and which is the "Many".
2. In the "One" class, add "collection navigation property". This is just a `List` of instances of the other class.
    <details><summary><code>ToDoList/Models/Category.cs</code></summary>

      ```c#
      using System.Collections.Generi   
      namespace ToDoList.Models
      {
        public class Category
        {
          public int CategoryId { get; set; }
          public string Name { get; set; }
          // The line below is new. This is the "collection navigation property"
          public List<Item> Items { get; set; }
        }
      }
      ```
    </details>

3. In the "Many" class, add "navigation properties". 

    <details><summary><code>ToDoList/Models/Item.cs</code></summary> 

      ```c#
      namespace ToDoList.Models
      {
        public class Item
        {
          public int ItemId { get; set; }
          public string Description { get; set; }
          // the next two lines are new. These are the "navigation properties" used to create the relationship
          public int CategoryId { get; set; }
          public Category Category { get; set; }
        }
      }
      ```
    </details>

4. Update the database. In the ToDoList, this just means adding a new column called `CategoryId` to the Items table.
5. Steps 1-4 are all that needs to happen to create the One-to-Many relationship between the models. Next, we need to give users the ability to make an association through the user-interface. In the ToDoList we add a form input to the pages that Create and Update `Items` so that users can make an association between each `Item` and a `Category`.
    - Within the `ItemsController` > `Create` action we add code to create a `SelectList` from the data in the Categories table. We use `ViewBag` to pass this `SelectList` to our views. 

      <details><summary><code>ToDoList/Controllers/ItemsController.cs</code></summary> 

        ```c#
          public ActionResult Create()
          {
            SelectList selectList = new SelectList(_db.Categories, "CategoryId", "Name");
            ViewBag.CategoryId = selectList;
            return View();
          }
        ```
      </details>

    - Our views use the `SelectList` passed down from the controller to create a dropdown selection.  
       <details><summary><code>ToDoList/Views/Create.cshtml</code></summary> 

      ```c#
      @{
        Layout = "_Layout";
      }
      
      @model ToDoList.Models.Item
      <p><strong>NOTE:</strong> You need to have at least one category before you can add an item!</p>
      <p>Go to @Html.ActionLink("this page", "Create", "Categories") to create a category.</p>
      <h4>Add a new item</h4>
      @using (Html.BeginForm())
      {
        @Html.LabelFor(model => model.Description)
        @Html.TextBoxFor(model => model.Description)
      
        @Html.LabelFor(model => model.Category)
        // The SelectList passed from the controller to the view via ViewBag is used to create a dropdown menu 
        // with an option for a user to select any existing category.
        @Html.DropDownList("CategoryId")
        
        <input type="submit" value="Add new item" />
      }
      <p>@Html.ActionLink("Show all items", "Index")</p>
      ```
      </details>
<hr />
    
# ADDITIONAL NOTES:

### HTML HELPER METHODS:
- `@Html.ActionLink` Example:
    - `@Html.ActionLink("See all items", "Index", "Items")`
    - This creates a link. Clicking the link will invoke the "Index" action   in `ItemsController.cs` 
    - The first argument ("See all Items") determines the text that will  appear on the webpage.
    - The second argument ("Index") specifies a controller action.
    - The third argument ("Items") specifies a controller and is optional (defaults to the controller associated with the current View).
    
- `@Html.ActionLink` Example 2:
  - `@Html.ActionLink($"{item.Description}", "Details", new { id = item.ItemId })`
  - Creates a link to the Details action of the controller with the same name as the Views subfolder that contains the view in which the link is rendered. If this link appeared in `Views/Items/Index.cshtml` it would it would invoke the `Details` action in `ItemsController.cs`. If it instead appeared in `Views/Categories/Index.cshtml`, it would invoke the `Details` action in `CategoriesController.cs`.
  - The code `new { id = item.ItemId }` creates an anonymous object with the property `id`; this is how .NET passes `id` to the `Details()` action. For .NET to route us to the details page for a specific Item, the property names of the object we pass in must match parameter names used by the target controller action. In this case, the `id` property of the anonymous object must match the parameter name in our `public ActionResult Details(int id)` in the `ItemsController`.

### MODEL DIRECTIVES:
  - `@model ToDoList.Models.Item`
    - Tells the view what type of data to expect to be passed from controller.
    - Required whenever using strongly typed HTML Helpers.
    - Only one model directive is allowed in each view.

### STRONGLY TYPED HELPERS:
  - Example:
    <details><summary><code>ToDoList/Views/Items/Create.cshtml</code></summary> 
    
    ```c#
    @using (Html.BeginForm())
    {
      @Html.LabelFor(model => model.Description)
      @Html.TextBoxFor(model => model.Description)
      <input type="submit" value="Add new item" />
    }
    ```
    </details>

  - By default, the form will create a `post` request to the route matching the filename it was called in. In the example above, this would create a `post` request to the `ItemsController` > `Create` action.
  - In the example above `LabelFor()` and `TextBoxFor()` are strongly typed helpers. Strongly typed helpers always have names that end with `For` to remind you that they are for a specific model.
  - `model => model.Description` is a lambda expression. The helpers need them to associate the parts of the form with a Model and its properties.
  - Strongly typed helpers provide error checking at compile time so they are recommended.
  - If you use a strongly typed helper you must include a model directive ie: `@model ToDoList.Models.Item`

### NAVIGATION PROPERTIES:
  - A navigation property is a property on one entity (like Category) that includes a reference to a related entity (like Item). EF Core uses navigation properties to recognize when there is a relationship between two entities.
  - In this case, EF Core sees that the `Categories` have an `Items` property of the type `List<Item>` and is able to understand that there is a relationship between `Category` and `Item`.
  - The `Items` property is more specifically categorized as a "collection navigation property" because it contains multiple entities. In this case, we have a collection (`List<>`) of multiple `Item` objects.
  - Navigation properties are never saved in the database. Instead, they are populated in our projects by EF Core from the data in the database.

### SelectList and @Html.DropDownList()

  - `@Html.DropDownList()` creates an HTML `<select>` dropdown menu and it's `<option>`'s. In order to make this happen we have to provide it with a particular data type, a `SelectList`.
  - We create a `SelectList` in our controllers like this: `SelectList selectList = new SelectList(_db.Categories, "CategoryId", "Name");`.
    - The first argument specifies the data that will populate our `<select>` dropdown's `<option>`s. In this example, we want to create an `<option>` for every category so we get the entire list of Categories from the database.
    - The second argument determines the `value` property of the every `<option>`.
    - The third argument determines the text of every `<option>` in our dropdown.
    - Altogether, we're saying that we want to create an `<option>` for every category that looks something like this: `<option value="1">Kitchen</option>` 
  - Next we pass the `SelectList` to our view via ViewBag.
  - Lastly in our view, we pass the SelectList object to `@Html.DropDownList()` to create the dropdown menu.

  - <details><summary><code>ItemsController.cs</code></summary> 
    
    ```c#
    public ActionResult Create()
    {
      // Create SelectList from the Categories table, 
      SelectList selectList = new SelectList(_db.Categories, "CategoryId", "Name");
      // Add a property (CategoryId) to the ViewBag object and use it to 
      // pass selectList to Views/Items/Create.cshtml
      ViewBag.CategoryId = selectList;
      return View();
    }
    ```
    </details>

  - <details><summary><code>Views/Items/Create.cs</code></summary> 
  
    ```c#
    @{
      Layout = "_Layout";
    }

    @model ToDoList.Models.Item
    <p><strong>NOTE:</strong> You need to have at least one category before you can add an item!</p>
    <p>Go to @Html.ActionLink("this page", "Create", "Categories") to create a category.</p>
    <h4>Add a new item</h4>
    @using (Html.BeginForm())
    {
      @Html.LabelFor(model => model.Description)
      @Html.TextBoxFor(model => model.Description)

      @Html.LabelFor(model => model.Category)
      // The line below creates the <select> dropdown menu using the data passed in from the controller and ViewBag.CategoryId
      @Html.DropDownList("CategoryId")

      <input type="submit" value="Add new item" />
    }
    <p>@Html.ActionLink("Show all items", "Index")</p>
    ```
  </details>

### EXPORTING A DATABASE:
- You'll be required to do this for the independent project. 
- [Follow this Lesson on LHTP](https://www.learnhowtoprogram.com/c-and-net-part-time/database-basics/creating-a-test-database-exporting-and-importing-databases-with-mysql-workbench)
