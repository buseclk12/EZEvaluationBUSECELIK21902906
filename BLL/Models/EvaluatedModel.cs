using System.ComponentModel;
using BLL.DAL;

namespace BLL.Models
{
    public class EvaluatedModel
    {
        public Evaluated Record { get; set; }

        [DisplayName("Name")]
        public string Name => Record.Name;

        [DisplayName("Surname")]
        public string Surname => Record.Surname;

        [DisplayName("Full Name")]
        public string FullName => $"{Record.Name} {Record.Surname}";
    }
}