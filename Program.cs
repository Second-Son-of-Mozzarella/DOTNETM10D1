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

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n\t Welcome to the Post n Blog database\n");
    Console.ForegroundColor = ConsoleColor.White;
    while (run)
    {

        Console.WriteLine(" Select an option to continue \n\t [1] to display all Blogs \n\t [2] to add a Blog\n\t [3] to create a post \n\t [4] to Display a Post \n\t [0] or Any other input to exit the program");
        string resp = Console.ReadLine();
         var db = new BloggingContext();
        

        switch (resp)
        {
            case "1":

                     // Display all Blogs from the database
                    var query = db.Blogs.OrderBy(b => b.BlogId);

                    Console.WriteLine("All blogs in the database:");
                    foreach (var item in query)
                    {
                        Console.WriteLine($"\n ID: {item.BlogId} \n Name: {item.Name} \n");
                    }
                break;

            case "2":
                    // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var blogname = Console.ReadLine();

                        var blog = new Blog { Name = blogname };

                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", blogname);


                break;

            case "3":




                        // Initial display and question about the post
                        var query2 = db.Blogs.OrderBy(b => b.BlogId);
                        Console.Write("What blog would you like to post to (Please enter blog ID): ");
                            foreach (var item in query2)
                            {
                                Console.WriteLine($"\n ID: {item.BlogId} \n Name: {item.Name} \n");
                            }
                        try{

                            // check if it goes through as a good input
                            int blogID = Int32.Parse(Console.ReadLine());

                            // check to see that the blog does exist
                            if(db.Blogs.Any(m => m.BlogId.Equals(blogID))){

                                    // enter title
                                    Console.Write("Enter a title for the post: ");
                                    string title = Console.ReadLine();
                                        // check for issues
                                        if(title != "" && title != null)
                                            {

                                                    Console.Write("Enter the content of the post: ");
                                                    string content = Console.ReadLine();


                                                    // you can do anything for contents
                                                    var post = new Post { Title =  title, Content = content, BlogId = blogID};

                                                    //just telling you its been added
                                                    logger.Info("Post added  - {title} to blog ID {blogID}", title, blogID);

                                            }else{
                                                Console.WriteLine("Invalid title"); // add error
                                            }

                                   
                            }else{
                                Console.WriteLine("Invalid blog ID"); // add error
                            }

                        }catch{
                            Console.WriteLine("Invalid blog ID"); // add error
                        }
                        
                        
                        

                        


                break;

            case "4":


                break;

            default:

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("\n\t Thanks for using the Post n Blog database\n");
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