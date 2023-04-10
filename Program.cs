using NLog;
using System.Linq;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

try
{
    bool run = true;
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n\t Welcome to the Post n Blog database");
    Console.ForegroundColor = ConsoleColor.White;
    while (run)
    {

        Console.WriteLine("\n Select an option to continue \n\t [1] to display all Blogs \n\t [2] to add a Blog\n\t [3] to create a post \n\t [4] to Display a Post \n\t [0] or Any other input to exit the program");
        string resp = Console.ReadLine();
        var db = new BloggingContext();



        switch (resp)
        {
            case "1": // Display blogs

                Console.Clear();

                // Display all Blogs from the database
                var query = db.Blogs.OrderBy(b => b.BlogId);

                if (query.Count() == 0)
                {
                    Console.WriteLine("No blogs in database");
                }
                else
                {
                    Console.WriteLine("\n Number of blogs in database: " + query.Count() + "\n All blogs in the database:");
                    foreach (var item in query)
                    {
                        Console.WriteLine($"\n\t ID: {item.BlogId} \n\t Name: {item.Name} \n");
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n \t\tPress any key to continue");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                }


                break;

            case "2": // add a blog
                Console.Clear();
                // Create and save a new Blog
                Console.WriteLine("Enter a name for a new Blog: ");
                var blogname = Console.ReadLine();

                if (blogname != "" && blogname != null)
                {
                    var blog = new Blog { Name = blogname };

                    db.AddBlog(blog);

                    logger.Info("Blog added - {name}", blogname);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n \t\tPress any key to continue");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    logger.Error("Invalid title (Title is either blank or null)");
                    Console.ForegroundColor = ConsoleColor.White;
                }




                break;

            case "3": // add a post
                Console.Clear();



                // Initial display and question about the post
                var query2 = db.Blogs.OrderBy(b => b.BlogId);
                Console.Write("What blog would you like to post to (Please enter blog ID): ");
                foreach (var item in query2)
                {
                    Console.WriteLine($"\n ID: {item.BlogId} \n Name: {item.Name} \n");
                }
                try
                {

                    // check if it goes through as a good input
                    int blogID = Int32.Parse(Console.ReadLine());

                    // check to see that the blog does exist
                    if (db.Blogs.Any(m => m.BlogId.Equals(blogID)))
                    {

                        var searchedBlog = db.Blogs.FirstOrDefault(b => b.BlogId == blogID);

                        // enter title
                        Console.WriteLine("Enter a title for the post: ");
                        string title = Console.ReadLine();
                        // check for issues
                        if (title != "" && title != null)
                        {

                            Console.WriteLine("Enter the content of the post: ");
                            string content = Console.ReadLine();


                            // you can do anything for contents
                            var post = new Post { Title = title, Content = content, BlogId = blogID, Blog = searchedBlog};

                            db.AddPost(post);

                            //just telling you its been added
                            logger.Info("Post added  - {title} to blog ID {blogID}", title, blogID);

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n \t\tPress any key to continue");
                            Console.ReadKey();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Clear();

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            logger.Error("Invalid title (Title is either blank or null)"); // an error for title issues
                            Console.ForegroundColor = ConsoleColor.White;

                        }


                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        logger.Error("Invalid blog ID (Blog with this ID does not exist)"); // an error if a Blog ID doesn't Exist
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                }
                catch
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    logger.Error("Invalid blog ID (Invalid Input)"); // an error if they typed in a non number
                    Console.ForegroundColor = ConsoleColor.White;
                }







                break;

            case "4":  // display posts
                Console.Clear();

                var query3 = db.Blogs.OrderBy(b => b.BlogId);
                Console.Write("What blogs posts would you like to see (Please enter blog ID): ");

                foreach (var item in query3)
                {
                    Console.WriteLine($"\n ID: {item.BlogId} \n Name: {item.Name} \n");
                }

                try
                {

                    int blogID2 = Int32.Parse(Console.ReadLine());

                    if (db.Blogs.Any(m => m.BlogId.Equals(blogID2)))
                    {

                        //var searchedBlog = db.Blogs.FirstOrDefault(b => b.BlogId == blogID2);
                        var posts = db.Posts.Where(p => p.BlogId == blogID2);

                        if (posts.Count() == 0)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            logger.Error("No posts under in this blog"); // an error if a Blog ID doesn't Exist
                            Console.ForegroundColor = ConsoleColor.White;

                        }
                        else
                        {

                            foreach (var post in posts)
                            {
                                Console.WriteLine($" \n -----------------------------------\n \tTitle: {post.Title} \n \tContent: {post.Content} \n ----------------------------------- \n");
                            }

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n \t\tPress any key to continue");
                            Console.ReadKey();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Clear();

                           
                        }




                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        logger.Error("Invalid blog ID (Blog with this ID does not exist)"); // an error if a Blog ID doesn't Exist
                        Console.ForegroundColor = ConsoleColor.White;
                    }


                }
                catch
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    logger.Error("Invalid blog ID (Invalid Input)"); // an error if they typed in a non number
                    Console.ForegroundColor = ConsoleColor.White;

                }


                break;

            default:

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("\n\t Thanks for using the Post n Blog database\n");
                Thread.Sleep(500);
                run = false;

                Console.ForegroundColor = ConsoleColor.White;

                break;
        }

    }





}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");
Console.Clear();