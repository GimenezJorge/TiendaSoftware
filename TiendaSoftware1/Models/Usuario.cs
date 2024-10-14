using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaSoftware1.Models;

public partial class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.", MinimumLength = 6)]
    public string Password { get; set; } = null!;

    [NotMapped]
    [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ReClave { get; set; } = null!;
    public int RolId { get; set; }

    public virtual Role Rol { get; set; } = null!;

    //relaciones
    public virtual ICollection<UsuarioDetalle> UsuarioDetalles { get; set; } = new List<UsuarioDetalle>();
    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

}