// Prompt (GPT):
// "Create a simple model class in C# named 'ClassInformationModel' with the following properties: 
// Id (auto-incremented), ClassName, StudentCount, Description. 
// Please also include validation attributes for each property ([Required], [Range], etc.)."



using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required.")]
        public string ClassName { get; set; } = "";

        [Range(1, 1000, ErrorMessage = "Student count must be between 1 and 1000.")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = "";
    }
}
