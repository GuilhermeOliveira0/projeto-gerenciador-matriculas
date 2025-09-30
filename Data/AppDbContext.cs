using MatriculasEAD.Models;
using Microsoft.EntityFrameworkCore;

namespace MatriculasEAD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da chave composta para Matricula
            modelBuilder.Entity<Matricula>()
                .HasKey(m => new { m.AlunoId, m.CursoId });

            // Configuração dos relacionamentos com DeleteBehavior.Restrict
            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Aluno)
                .WithMany(a => a.Matriculas)
                .HasForeignKey(m => m.AlunoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Curso)
                .WithMany(c => c.Matriculas)
                .HasForeignKey(m => m.CursoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração de precisão para campos decimais
            modelBuilder.Entity<Curso>()
                .Property(c => c.PrecoBase)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Matricula>()
                .Property(m => m.PrecoPago)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Matricula>()
                .Property(m => m.NotaFinal)
                .HasPrecision(3, 1);

            // Configuração de índices para melhor performance nas buscas
            modelBuilder.Entity<Aluno>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<Aluno>()
                .HasIndex(a => a.Nome);

            modelBuilder.Entity<Curso>()
                .HasIndex(c => c.Titulo);

            // Configuração de conversão automática do enum
            modelBuilder.Entity<Matricula>()
                .Property(m => m.Status)
                .HasConversion<string>();

            // Configuração do campo Data para usar UTC
            modelBuilder.Entity<Matricula>()
                .Property(m => m.Data)
                .HasColumnType("timestamp with time zone");
        }
    }
}