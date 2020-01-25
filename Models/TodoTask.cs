using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoMVC.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }

        [ForeignKey("Topic")]
        public int TopicId { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
    }
}