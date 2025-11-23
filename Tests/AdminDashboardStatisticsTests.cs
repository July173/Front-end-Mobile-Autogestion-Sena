using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;
using Xunit;
using FluentAssertions;

namespace AutogestionSena.MAUI.Tests
{
    /// <summary>
    /// Tests para verificar la lógica de estadísticas del dashboard de administrador
    /// </summary>
    public class AdminDashboardStatisticsTests
    {
        [Fact]
        public void DeberiaContarAprendicesActivos()
        {
            // Arrange: Crear lista de aprendices con algunos activos y otros inactivos
            var apprentices = new List<ApprenticeSimpleDto>
            {
                new ApprenticeSimpleDto { Id = 1, Person = 101, Active = true },
                new ApprenticeSimpleDto { Id = 2, Person = 102, Active = true },
                new ApprenticeSimpleDto { Id = 3, Person = 103, Active = false },
                new ApprenticeSimpleDto { Id = 4, Person = 104, Active = true },
                new ApprenticeSimpleDto { Id = 5, Person = 105, Active = false }
            };

            // Act: Contar aprendices activos
            var activeCount = apprentices.Count(a => a.Active);

            // Assert: Debe haber 3 aprendices activos
            activeCount.Should().Be(3);
        }

        [Fact]
        public void DeberiaContarSolicitudesSinAsignar()
        {
            // Arrange: Crear lista de solicitudes con diferentes estados
            var assignments = new List<AssignmentRequestDto>
            {
                new AssignmentRequestDto { Id = 1, RequestState = "SIN_ASIGNAR" },
                new AssignmentRequestDto { Id = 2, RequestState = "ASIGNADO" },
                new AssignmentRequestDto { Id = 3, RequestState = "SIN_ASIGNAR" },
                new AssignmentRequestDto { Id = 4, RequestState = "APROBADO" },
                new AssignmentRequestDto { Id = 5, RequestState = "SIN_ASIGNAR" }
            };

            // Act: Contar solicitudes sin asignar
            var unassignedCount = assignments.Count(a => a.RequestState == "SIN_ASIGNAR");

            // Assert: Debe haber 3 solicitudes sin asignar
            unassignedCount.Should().Be(3);
        }

        [Fact]
        public void DeberiaContarSolicitudesAsignadas()
        {
            // Arrange: Crear lista de solicitudes
            var assignments = new List<AssignmentRequestDto>
            {
                new AssignmentRequestDto { Id = 1, RequestState = "SIN_ASIGNAR" },
                new AssignmentRequestDto { Id = 2, RequestState = "ASIGNADO" },
                new AssignmentRequestDto { Id = 3, RequestState = "SIN_ASIGNAR" },
                new AssignmentRequestDto { Id = 4, RequestState = "APROBADO" },
                new AssignmentRequestDto { Id = 5, RequestState = "RECHAZADO" }
            };

            // Act: Contar todas las asignadas (las que NO están sin asignar)
            var assignedCount = assignments.Count(a => a.RequestState != "SIN_ASIGNAR");

            // Assert: Debe haber 3 solicitudes asignadas
            assignedCount.Should().Be(3);
        }

        [Fact]
        public void DeberiaAgruparAsignacionesPorEstado()
        {
            // Arrange: Crear lista de solicitudes con varios estados
            var assignments = new List<AssignmentRequestDto>
            {
                new AssignmentRequestDto { Id = 1, RequestState = "SIN_ASIGNAR" },
                new AssignmentRequestDto { Id = 2, RequestState = "SIN_ASIGNAR" },
                new AssignmentRequestDto { Id = 3, RequestState = "ASIGNADO" },
                new AssignmentRequestDto { Id = 4, RequestState = "APROBADO" },
                new AssignmentRequestDto { Id = 5, RequestState = "APROBADO" },
                new AssignmentRequestDto { Id = 6, RequestState = "APROBADO" },
                new AssignmentRequestDto { Id = 7, RequestState = "RECHAZADO" }
            };

            // Act: Agrupar por estado (lógica para gráfica de barras)
            var statusCounts = assignments
                .GroupBy(a => a.RequestState ?? "DESCONOCIDO")
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            // Assert: Verificar agrupación
            statusCounts.Should().HaveCount(4);
            statusCounts.First().Status.Should().Be("APROBADO");
            statusCounts.First().Count.Should().Be(3);
            statusCounts.Should().Contain(x => x.Status == "SIN_ASIGNAR" && x.Count == 2);
            statusCounts.Should().Contain(x => x.Status == "ASIGNADO" && x.Count == 1);
            statusCounts.Should().Contain(x => x.Status == "RECHAZADO" && x.Count == 1);
        }

        [Fact]
        public void DeberiaParsearFechaSolicitudCorrectamente()
        {
            // Arrange: Crear solicitud con fecha
            var assignment = new AssignmentRequestDto
            {
                Id = 1,
                FechaSolicitud = "2025-11-15T10:30:00"
            };

            // Act: Obtener fecha parseada
            var createdAt = assignment.CreatedAt;

            // Assert: Fecha debe estar parseada correctamente
            createdAt.Should().NotBeNull();
            createdAt.Value.Year.Should().Be(2025);
            createdAt.Value.Month.Should().Be(11);
            createdAt.Value.Day.Should().Be(15);
        }

        [Fact]
        public void DeberiaRetornarNullSiFechaSolicitudEsInvalida()
        {
            // Arrange: Crear solicitud con fecha inválida
            var assignment = new AssignmentRequestDto
            {
                Id = 1,
                FechaSolicitud = "fecha-invalida"
            };

            // Act: Obtener fecha parseada
            var createdAt = assignment.CreatedAt;

            // Assert: Debe retornar null
            createdAt.Should().BeNull();
        }

        [Fact]
        public void DeberiaAgruparAsignacionesPorMes()
        {
            // Arrange: Crear solicitudes de diferentes meses
            var assignments = new List<AssignmentRequestDto>
            {
                new AssignmentRequestDto { Id = 1, FechaSolicitud = "2025-09-15T10:00:00" },
                new AssignmentRequestDto { Id = 2, FechaSolicitud = "2025-09-20T11:00:00" },
                new AssignmentRequestDto { Id = 3, FechaSolicitud = "2025-10-05T09:00:00" },
                new AssignmentRequestDto { Id = 4, FechaSolicitud = "2025-10-12T14:00:00" },
                new AssignmentRequestDto { Id = 5, FechaSolicitud = "2025-10-25T16:00:00" },
                new AssignmentRequestDto { Id = 6, FechaSolicitud = "2025-11-03T08:00:00" }
            };

            // Act: Agrupar por mes (lógica para gráfica de líneas)
            var byMonth = assignments
                .Where(a => a.CreatedAt.HasValue)
                .GroupBy(a => new { Year = a.CreatedAt!.Value.Year, Month = a.CreatedAt.Value.Month })
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            // Assert: Verificar agrupación por mes
            byMonth.Should().HaveCount(3);
            byMonth[0].Date.Month.Should().Be(9);
            byMonth[0].Count.Should().Be(2);
            byMonth[1].Date.Month.Should().Be(10);
            byMonth[1].Count.Should().Be(3);
            byMonth[2].Date.Month.Should().Be(11);
            byMonth[2].Count.Should().Be(1);
        }

        [Fact]
        public void DeberiaTomarUltimos6Meses()
        {
            // Arrange: Crear solicitudes de muchos meses
            var assignments = new List<AssignmentRequestDto>();
            for (int i = 1; i <= 12; i++)
            {
                assignments.Add(new AssignmentRequestDto
                {
                    Id = i,
                    FechaSolicitud = new DateTime(2025, i, 1).ToString("yyyy-MM-ddTHH:mm:ss")
                });
            }

            // Act: Agrupar y tomar últimos 6 meses
            var byMonth = assignments
                .Where(a => a.CreatedAt.HasValue)
                .GroupBy(a => new { Year = a.CreatedAt!.Value.Year, Month = a.CreatedAt.Value.Month })
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .TakeLast(6)
                .ToList();

            // Assert: Solo debe haber 6 meses
            byMonth.Should().HaveCount(6);
            byMonth.First().Date.Month.Should().Be(7); // Julio
            byMonth.Last().Date.Month.Should().Be(12); // Diciembre
        }

        [Fact]
        public void DeberiaConvertirEstadosANombresCortos()
        {
            // Arrange: Estados largos
            var estados = new Dictionary<string, string>
            {
                { "SIN_ASIGNAR", "Sin Asig." },
                { "ASIGNADO", "Asignado" },
                { "APROBADO", "Aprobado" },
                { "RECHAZADO", "Rechazado" },
                { "EN_PROCESO", "En Proc." }
            };

            // Act & Assert: Verificar conversión
            foreach (var (estado, esperado) in estados)
            {
                var corto = GetShortStatus(estado);
                corto.Should().Be(esperado);
            }
        }

        [Fact]
        public void DeberiaManejarEstadosNulosODesconocidos()
        {
            // Arrange
            var assignments = new List<AssignmentRequestDto>
            {
                new AssignmentRequestDto { Id = 1, RequestState = null },
                new AssignmentRequestDto { Id = 2, RequestState = "" },
                new AssignmentRequestDto { Id = 3, RequestState = "ESTADO_MUY_LARGO_DESCONOCIDO" }
            };

            // Act: Agrupar (null debe convertirse a "DESCONOCIDO")
            var statusCounts = assignments
                .GroupBy(a => a.RequestState ?? "DESCONOCIDO")
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            // Assert
            statusCounts.Should().Contain(x => x.Status == "DESCONOCIDO");
            
            // Verificar nombres cortos
            GetShortStatus(null).Should().Be("N/A");
            GetShortStatus("").Should().Be("");
            GetShortStatus("ESTADO_MUY_LARGO_DESCONOCIDO").Should().Be("ESTADO_M");
        }

        // Método auxiliar (mismo que en AdminDashboardPage)
        private string GetShortStatus(string? status)
        {
            return status switch
            {
                "SIN_ASIGNAR" => "Sin Asig.",
                "ASIGNADO" => "Asignado",
                "APROBADO" => "Aprobado",
                "RECHAZADO" => "Rechazado",
                "EN_PROCESO" => "En Proc.",
                _ => status?.Length > 8 ? status.Substring(0, 8) : status ?? "N/A"
            };
        }
    }
}
