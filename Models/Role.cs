namespace R12VIS.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Access { get; set; }

        public void ToUpper()
        {
            Title = Title.ToUpper();
            Access = Access.ToUpper();
        }
    }
}