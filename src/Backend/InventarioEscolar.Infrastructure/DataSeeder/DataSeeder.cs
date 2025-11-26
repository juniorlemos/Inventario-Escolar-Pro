using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Enums;
using InventarioEscolar.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventarioEscolar.Infrastructure.DataSeeder
{
    public static class DataSeeder
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<InventarioEscolarProDBContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await context.Database.MigrateAsync();

          


            var escola = await context.Schools.FirstOrDefaultAsync();
            if (escola == null)
            {
                escola = new School
                {
                    Name = "Escola Central",
                    Address = "Rua Principal, 123",
                };

                await context.Schools.AddAsync(escola);
                await context.SaveChangesAsync();
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
            }

   
            var adminUser = await userManager.FindByEmailAsync("admin@escola.com");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@escola.com",
                    Email = "admin@escola.com",
                    EmailConfirmed = true,
                    SchoolId = escola.Id
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (!result.Succeeded)
                {
                    throw new Exception("Falha ao criar admin: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

       
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

                var categorias = new[]
                {
                    new Category { Name = "Informática", Description = "Equipamentos e acessórios de tecnologia utilizados nas aulas e escritórios.", School = escola },
                    new Category { Name = "Mobiliário", Description = "Mesas, cadeiras, armários e demais móveis utilizados nos ambientes escolares.", School = escola },
                    new Category { Name = "Didático", Description = "Materiais e recursos pedagógicos usados em sala de aula.", School = escola },
                    new Category { Name = "Eletrodomésticos", Description = "Aparelhos elétricos de uso cotidiano, como geladeiras e ventiladores.", School = escola },
                    new Category { Name = "Audiovisual", Description = "Equipamentos de som e imagem usados em apresentações e eventos.", School = escola },
                    new Category { Name = "Limpeza", Description = "Itens e equipamentos usados na manutenção e higienização da escola.", School = escola },
                    new Category { Name = "Manutenção", Description = "Ferramentas e materiais utilizados para conserto e reparos.", School = escola },
                    new Category { Name = "Administração", Description = "Equipamentos e materiais voltados à gestão e escritório da escola.", School = escola },
                    new Category { Name = "Laboratório", Description = "Instrumentos e equipamentos usados em experimentos científicos.", School = escola },
                    new Category { Name = "Esporte e Lazer", Description = "Materiais e equipamentos destinados a atividades esportivas e recreativas.", School = escola }
                };

                // Salas
                var salas = new[]
                {
                    new RoomLocation { Name = "Laboratório de Informática", Building = "Bloco A", Description = "Espaço destinado às aulas de computação e tecnologia.", School = escola },
                    new RoomLocation { Name = "Biblioteca", Building = "Bloco B", Description = "Local para leitura, estudo e empréstimo de livros.", School = escola },
                    new RoomLocation { Name = "Diretoria", Building = "Bloco C", Description = "Sala administrativa onde atua a equipe de gestão escolar.", School = escola },
                    new RoomLocation { Name = "Sala dos Professores", Building = "Bloco C", Description = "Espaço de convivência e trabalho dos professores.", School = escola },
                    new RoomLocation { Name = "Secretaria", Building = "Bloco D", Description = "Local de atendimento e organização de documentos escolares.", School = escola },
                    new RoomLocation { Name = "Refeitório", Building = "Bloco E", Description = "Área destinada à alimentação dos alunos e funcionários.", School = escola },
                    new RoomLocation { Name = "Quadra Poliesportiva", Building = "Área Externa", Description = "Espaço para atividades esportivas e recreativas.", School = escola },
                    new RoomLocation { Name = "Sala de Ciências", Building = "Bloco F", Description = "Ambiente equipado para aulas práticas de ciências e biologia.", School = escola },
                    new RoomLocation { Name = "Auditório", Building = "Bloco G", Description = "Espaço usado para eventos, reuniões e apresentações.", School = escola },
                    new RoomLocation { Name = "Depósito de Materiais", Building = "Bloco H", Description = "Área destinada ao armazenamento de equipamentos e materiais.", School = escola }
                };

                await context.Categories.AddRangeAsync(categorias);
                await context.RoomLocations.AddRangeAsync(salas);
                await context.SaveChangesAsync();

                // =============================
                // ATIVOS
                // =============================
                var ativos = new[]
                {
                // ==== Informática ====
                    new Asset { Name = "Computador Dell Optiplex", Description = "Desktop Core i5 com 8GB RAM", PatrimonyCode = 1001, AcquisitionValue = 3500.00m, ConservationState = ConservationState.Novo, CategoryId = categorias.First(c => c.Name == "Informática").Id, RoomLocationId = salas.First(s => s.Name == "Laboratório de Informática").Id, SchoolId = escola.Id },
                    new Asset { Name = "Projetor Epson", Description = "Projetor multimídia 3000 lumens", PatrimonyCode = 1005, AcquisitionValue = 2100.00m, ConservationState = ConservationState.Danificado, CategoryId = categorias.First(c => c.Name == "Informática").Id, RoomLocationId = salas.First(s => s.Name == "Auditório").Id, SchoolId = escola.Id },
                    new Asset { Name = "Impressora HP LaserJet", Description = "Impressora a laser monocromática", PatrimonyCode = 1004, AcquisitionValue = 1200.00m, ConservationState = ConservationState.Regular, CategoryId = categorias.First(c => c.Name == "Informática").Id, RoomLocationId = salas.First(s => s.Name == "Secretaria").Id, SchoolId = escola.Id },
                    new Asset { Name = "Notebook Acer Aspire", Description = "Notebook i5 8GB RAM SSD 256GB", PatrimonyCode = 1010, AcquisitionValue = 4200.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Informática").Id, RoomLocationId = salas.First(s => s.Name == "Diretoria").Id, SchoolId = escola.Id },
                    new Asset { Name = "Switch TP-Link 24 Portas", Description = "Switch gerenciável Gigabit", PatrimonyCode = 1011, AcquisitionValue = 950.00m, ConservationState = ConservationState.Novo, CategoryId = categorias.First(c => c.Name == "Informática").Id, RoomLocationId = salas.First(s => s.Name == "Laboratório de Informática").Id, SchoolId = escola.Id },

                // ==== Mobiliário ====
                    new Asset { Name = "Cadeira Giratória", Description = "Cadeira de escritório preta com rodinhas", PatrimonyCode = 2002, AcquisitionValue = 450.00m, ConservationState = ConservationState.Regular, CategoryId = categorias.First(c => c.Name == "Mobiliário").Id, RoomLocationId = salas.First(s => s.Name == "Sala dos Professores").Id, SchoolId = escola.Id },
                    new Asset { Name = "Mesa de Madeira", Description = "Mesa retangular grande de madeira clara", PatrimonyCode = 2001, AcquisitionValue = 700.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Mobiliário").Id, RoomLocationId = salas.First(s => s.Name == "Diretoria").Id, SchoolId = escola.Id },
                    new Asset { Name = "Armário de Aço", Description = "Armário cinza 2 portas", PatrimonyCode = 2003, AcquisitionValue = 890.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Mobiliário").Id, RoomLocationId = salas.First(s => s.Name == "Secretaria").Id, SchoolId = escola.Id },
                    new Asset { Name = "Estante de Livros", Description = "Estante de madeira grande", PatrimonyCode = 2004, AcquisitionValue = 600.00m, ConservationState = ConservationState.Novo, CategoryId = categorias.First(c => c.Name == "Mobiliário").Id, RoomLocationId = salas.First(s => s.Name == "Biblioteca").Id, SchoolId = escola.Id },
                    new Asset { Name = "Banco Escolar Duplo", Description = "Banco de madeira com encosto", PatrimonyCode = 2006, AcquisitionValue = 400.00m, ConservationState = ConservationState.Regular, CategoryId = categorias.First(c => c.Name == "Mobiliário").Id, RoomLocationId = salas.First(s => s.Name == "Sala de Ciências").Id, SchoolId = escola.Id },

                // ==== Didático ====
                    new Asset { Name = "Quadro Branco", Description = "Quadro branco magnético 2x1m", PatrimonyCode = 3002, AcquisitionValue = 300.00m, ConservationState = ConservationState.Novo, CategoryId = categorias.First(c => c.Name == "Didático").Id, RoomLocationId = salas.First(s => s.Name == "Laboratório de Informática").Id, SchoolId = escola.Id },
                    new Asset { Name = "Microscópio Escolar", Description = "Microscópio óptico simples", PatrimonyCode = 3005, AcquisitionValue = 850.00m, ConservationState = ConservationState.Regular, CategoryId = categorias.First(c => c.Name == "Didático").Id, RoomLocationId = salas.First(s => s.Name == "Sala de Ciências").Id, SchoolId = escola.Id },
                    new Asset { Name = "Mapa Mundi", Description = "Mapa político e físico em painel", PatrimonyCode = 3006, AcquisitionValue = 200.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Didático").Id, RoomLocationId = salas.First(s => s.Name == "Biblioteca").Id, SchoolId = escola.Id },
                    new Asset { Name = "Kit de Química", Description = "Conjunto de tubos e reagentes básicos", PatrimonyCode = 3007, AcquisitionValue = 950.00m, ConservationState = ConservationState.Novo, CategoryId = categorias.First(c => c.Name == "Didático").Id, RoomLocationId = salas.First(s => s.Name == "Sala de Ciências").Id, SchoolId = escola.Id },

                // ==== Audiovisual ====
                    new Asset { Name = "Caixa de Som JBL", Description = "Caixa de som Bluetooth 200W", PatrimonyCode = 4001, AcquisitionValue = 1100.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Audiovisual").Id, RoomLocationId = salas.First(s => s.Name == "Auditório").Id, SchoolId = escola.Id },
                    new Asset { Name = "Televisor Samsung 55\"", Description = "Smart TV UHD 4K", PatrimonyCode = 4003, AcquisitionValue = 3500.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Audiovisual").Id, RoomLocationId = salas.First(s => s.Name == "Biblioteca").Id, SchoolId = escola.Id },
                    new Asset { Name = "Microfone Sem Fio", Description = "Kit duplo com receptor UHF", PatrimonyCode = 4005, AcquisitionValue = 580.00m, ConservationState = ConservationState.Novo, CategoryId = categorias.First(c => c.Name == "Audiovisual").Id, RoomLocationId = salas.First(s => s.Name == "Auditório").Id, SchoolId = escola.Id },

                // ==== Eletrodomésticos ====
                    new Asset { Name = "Geladeira Consul", Description = "Refrigerador duplex 300L", PatrimonyCode = 7001, AcquisitionValue = 2500.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Eletrodomésticos").Id, RoomLocationId = salas.First(s => s.Name == "Refeitório").Id, SchoolId = escola.Id },
                    new Asset { Name = "Bebedouro Elétrico", Description = "Bebedouro com refrigeração e filtragem", PatrimonyCode = 7002, AcquisitionValue = 800.00m, ConservationState = ConservationState.Novo, CategoryId = categorias.First(c => c.Name == "Eletrodomésticos").Id, RoomLocationId = salas.First(s => s.Name == "Refeitório").Id, SchoolId = escola.Id },
                    new Asset { Name = "Ventilador de Parede", Description = "Ventilador oscilante 60cm", PatrimonyCode = 7003, AcquisitionValue = 300.00m, ConservationState = ConservationState.Regular, CategoryId = categorias.First(c => c.Name == "Eletrodomésticos").Id, RoomLocationId = salas.First(s => s.Name == "Sala dos Professores").Id, SchoolId = escola.Id },
                    new Asset { Name = "Micro-ondas Brastemp", Description = "Micro-ondas digital 30L", PatrimonyCode = 7004, AcquisitionValue = 600.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Eletrodomésticos").Id, RoomLocationId = salas.First(s => s.Name == "Refeitório").Id, SchoolId = escola.Id },

                // ==== Administração ====
                    new Asset { Name = "Telefone IP", Description = "Telefone digital com viva-voz", PatrimonyCode = 5003, AcquisitionValue = 300.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Administração").Id, RoomLocationId = salas.First(s => s.Name == "Diretoria").Id, SchoolId = escola.Id },
                    new Asset { Name = "Computador Administrativo", Description = "PC para controle de matrículas", PatrimonyCode = 5004, AcquisitionValue = 2800.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Administração").Id, RoomLocationId = salas.First(s => s.Name == "Secretaria").Id, SchoolId = escola.Id },

                // ==== Esporte e Lazer ====
                    new Asset { Name = "Bola de Futebol", Description = "Bola oficial de couro sintético", PatrimonyCode = 8001, AcquisitionValue = 120.00m, ConservationState = ConservationState.Novo, CategoryId = categorias.First(c => c.Name == "Esporte e Lazer").Id, RoomLocationId = salas.First(s => s.Name == "Quadra Poliesportiva").Id, SchoolId = escola.Id },
                    new Asset { Name = "Rede de Vôlei", Description = "Rede oficial de voleibol", PatrimonyCode = 8002, AcquisitionValue = 250.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Esporte e Lazer").Id, RoomLocationId = salas.First(s => s.Name == "Quadra Poliesportiva").Id, SchoolId = escola.Id },
                    new Asset { Name = "Tabela de Basquete", Description = "Estrutura metálica com aro e tabela", PatrimonyCode = 8004, AcquisitionValue = 1800.00m, ConservationState = ConservationState.Regular, CategoryId = categorias.First(c => c.Name == "Esporte e Lazer").Id, RoomLocationId = salas.First(s => s.Name == "Quadra Poliesportiva").Id, SchoolId = escola.Id },
                    new Asset { Name = "Troféu de Campeão Escolar", Description = "Troféu de metal dourado", PatrimonyCode = 8005, AcquisitionValue = 180.00m, ConservationState = ConservationState.Bom, CategoryId = categorias.First(c => c.Name == "Esporte e Lazer").Id, RoomLocationId = salas.First(s => s.Name == "Diretoria").Id, SchoolId = escola.Id }
                };


                await context.Assets.AddRangeAsync(ativos);
                await context.SaveChangesAsync();

                // =============================
                // MOVIMENTAÇÕES DE ATIVOS
                // =============================
                var movimentacoes = new[]
                {
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Computador Dell Optiplex"), FromRoom = salas.First(s => s.Name == "Diretoria"), ToRoom = salas.First(s => s.Name == "Laboratório de Informática"), Responsible = "João Silva", MovedAt = DateTime.UtcNow.AddDays(-30), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Projetor Epson"), FromRoom = salas.First(s => s.Name == "Laboratório de Informática"), ToRoom = salas.First(s => s.Name == "Auditório"), Responsible = "Maria Oliveira", MovedAt = DateTime.UtcNow.AddDays(-25), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Cadeira Giratória"), FromRoom = salas.First(s => s.Name == "Diretoria"), ToRoom = salas.First(s => s.Name == "Sala dos Professores"), Responsible = "Carlos Mendes", MovedAt = DateTime.UtcNow.AddDays(-22), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Geladeira Consul"), FromRoom = salas.First(s => s.Name == "Depósito de Materiais"), ToRoom = salas.First(s => s.Name == "Refeitório"), Responsible = "Ana Paula", MovedAt = DateTime.UtcNow.AddDays(-20), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Caixa de Som JBL"), FromRoom = salas.First(s => s.Name == "Biblioteca"), ToRoom = salas.First(s => s.Name == "Auditório"), Responsible = "Fernanda Lima", MovedAt = DateTime.UtcNow.AddDays(-18), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Impressora HP LaserJet"), FromRoom = salas.First(s => s.Name == "Secretaria"), ToRoom = salas.First(s => s.Name == "Diretoria"), Responsible = "Roberto Souza", MovedAt = DateTime.UtcNow.AddDays(-15), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Mesa de Madeira"), FromRoom = salas.First(s => s.Name == "Diretoria"), ToRoom = salas.First(s => s.Name == "Sala dos Professores"), Responsible = "Juliana Freitas", MovedAt = DateTime.UtcNow.AddDays(-14), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Ventilador de Parede"), FromRoom = salas.First(s => s.Name == "Depósito de Materiais"), ToRoom = salas.First(s => s.Name == "Sala dos Professores"), Responsible = "Marcos Lima", MovedAt = DateTime.UtcNow.AddDays(-12), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Televisor Samsung 55\""), FromRoom = salas.First(s => s.Name == "Biblioteca"), ToRoom = salas.First(s => s.Name == "Auditório"), Responsible = "Lucas Rocha", MovedAt = DateTime.UtcNow.AddDays(-10), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Bola de Futebol"), FromRoom = salas.First(s => s.Name == "Depósito de Materiais"), ToRoom = salas.First(s => s.Name == "Quadra Poliesportiva"), Responsible = "Paulo Ferreira", MovedAt = DateTime.UtcNow.AddDays(-9), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Rede de Vôlei"), FromRoom = salas.First(s => s.Name == "Depósito de Materiais"), ToRoom = salas.First(s => s.Name == "Quadra Poliesportiva"), Responsible = "Ricardo Alves", MovedAt = DateTime.UtcNow.AddDays(-8), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Microscópio Escolar"), FromRoom = salas.First(s => s.Name == "Depósito de Materiais"), ToRoom = salas.First(s => s.Name == "Sala de Ciências"), Responsible = "Joana Santos", MovedAt = DateTime.UtcNow.AddDays(-7), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Telefone IP"), FromRoom = salas.First(s => s.Name == "Secretaria"), ToRoom = salas.First(s => s.Name == "Diretoria"), Responsible = "Patrícia Gomes", MovedAt = DateTime.UtcNow.AddDays(-6), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Bebedouro Elétrico"), FromRoom = salas.First(s => s.Name == "Depósito de Materiais"), ToRoom = salas.First(s => s.Name == "Refeitório"), Responsible = "Daniel Castro", MovedAt = DateTime.UtcNow.AddDays(-5), SchoolId = escola.Id },
                    new AssetMovement { Asset = ativos.First(a => a.Name == "Quadro Branco"), FromRoom = salas.First(s => s.Name == "Depósito de Materiais"), ToRoom = salas.First(s => s.Name == "Laboratório de Informática"), Responsible = "Rafael Silva", MovedAt = DateTime.UtcNow.AddDays(-3), SchoolId = escola.Id }
                };

                await context.AssetMovements.AddRangeAsync(movimentacoes);
                await context.SaveChangesAsync();
            }

       
      
        }

    }
}
