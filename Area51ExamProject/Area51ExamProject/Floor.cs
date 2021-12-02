using Area51ExamProject.Enums;
namespace Area51ExamProject
{
    public class Floor
    {
        public string Name { get; set; }
        public SecurityLevel RequiredSecLevel { get; set; }

        public Floor(string name, SecurityLevel requiredSecLevel)
        {
            Name = name;
            RequiredSecLevel = requiredSecLevel;
        }
    }
}
