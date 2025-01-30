namespace CORE.APP.Domain
{
    public abstract class Entity
    {
        //private int id; // variable, field

        //public int GetId() // function, method, behavior
        //{
        //    return id;
        //}

        //public void SetId(int id)
        //{
        //    this.id = id;
        //}

        public int Id { get; set; } // property, Entity entity = new Entity() { Id = 17 };

        protected Entity()
        {
        }

        protected Entity(int id) // Entity entity = new Entity(19);
        {
            Id = id;
        }
    }
}
