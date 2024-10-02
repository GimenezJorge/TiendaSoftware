using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TiendaSoftware1.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public int RolId { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public virtual Role Rol { get; set; } = null!;

        // Agregar ReClave para la validación en el registro
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ReClave { get; set; }
    }
}