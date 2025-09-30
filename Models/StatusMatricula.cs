using System.ComponentModel.DataAnnotations;

namespace MatriculasEAD.Models;

public enum StatusMatricula
{
    [Display(Name = "Ativa")]
    Ativa,
    
    [Display(Name = "Concluída")]
    Concluida,
    
    [Display(Name = "Cancelada")]
    Cancelada
}
