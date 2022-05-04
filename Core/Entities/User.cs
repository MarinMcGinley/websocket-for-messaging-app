namespace Core.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        //[Datatype(DataType.Password)]
        public string Password { get; set; }
        public string Name { get; set; }
    }
}