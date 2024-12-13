//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class UserAccount
    {
        private string _status;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserAccount()
        {
            this.Audit = new HashSet<Audit>();
            this.Product = new HashSet<Product>();
            this.SecurityLog = new HashSet<SecurityLog>();

            // Valor predeterminado para Status
            this.Status = "Active";
        }

        public int UserID { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [RegularExpression("Admin|Editor|Viewer", ErrorMessage = "El rol debe ser 'Admin', 'Editor' o 'Viewer'.")]
        public string Role { get; set; }

        public Nullable<bool> AccountLocked { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("Active|Inactive", ErrorMessage = "El estado debe ser 'Active' o 'Inactive'.")]
        public string Status
        {
            get => _status;
            set
            {
                if (value != "Active" && value != "Inactive")
                {
                    throw new ArgumentException("El valor de 'Status' debe ser 'Active' o 'Inactive'.");
                }
                _status = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Audit> Audit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SecurityLog> SecurityLog { get; set; }
    }
}
