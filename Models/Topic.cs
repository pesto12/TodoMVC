using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace TodoMVC.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<TodoTask> Tasks { get; set; }
    }
}