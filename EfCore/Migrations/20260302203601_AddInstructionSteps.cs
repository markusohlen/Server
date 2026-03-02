using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructionSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Meals");

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    MealId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructionSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StepNumber = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    MealId = table.Column<Guid>(type: "uuid", nullable: false),
                    SectionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructionSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructionSteps_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructionSteps_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "InstructionIngredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Measure = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    InstructionStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientId = table.Column<Guid>(type: "uuid", nullable: false),
                    SectionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructionIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructionIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructionIngredients_InstructionSteps_InstructionStepId",
                        column: x => x.InstructionStepId,
                        principalTable: "InstructionSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructionIngredients_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructionIngredients_IngredientId",
                table: "InstructionIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructionIngredients_InstructionStepId",
                table: "InstructionIngredients",
                column: "InstructionStepId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructionIngredients_SectionId",
                table: "InstructionIngredients",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructionSteps_MealId",
                table: "InstructionSteps",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructionSteps_SectionId",
                table: "InstructionSteps",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_MealId",
                table: "Sections",
                column: "MealId");

            // Seed data: Meals
            migrationBuilder.Sql(@"
INSERT INTO ""Meals"" (""Id"", ""Name"", ""Portions"") VALUES
('867fd29c-2fa3-4332-99c2-7bd5b52e64f5', 'Spaghetti Bolognese', 0),
('3ccf980c-54f2-41ba-9c56-92f9857ef58c', 'Chicken Stir Fry', 0),
('f81d6693-8dd4-4a4c-a135-31a49f5abc62', 'Grilled Cheese Sandwich', 0),
('65e38586-6b3a-468f-a34c-f41802af246a', 'Taco Salad', 0),
('b0daf6ea-54b9-49ee-b3e9-64675c668b96', 'Omelette', 0);
");

            // Seed data: Sections
            migrationBuilder.Sql(@"
INSERT INTO ""Sections"" (""Id"", ""Name"", ""SortOrder"", ""MealId"") VALUES
-- Spaghetti Bolognese
('c1000001-0001-0001-0001-000000000001', 'Kött', 0, '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
('c1000001-0001-0001-0001-000000000002', 'Sås', 1, '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
('c1000001-0001-0001-0001-000000000003', 'Pasta', 2, '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
-- Chicken Stir Fry
('c1000002-0001-0001-0001-000000000001', 'Kycklingen', 0, '3ccf980c-54f2-41ba-9c56-92f9857ef58c'),
('c1000002-0001-0001-0001-000000000002', 'Grönsaker', 1, '3ccf980c-54f2-41ba-9c56-92f9857ef58c'),
('c1000002-0001-0001-0001-000000000003', 'Ris', 2, '3ccf980c-54f2-41ba-9c56-92f9857ef58c'),
-- Grilled Cheese Sandwich
('c1000003-0001-0001-0001-000000000001', 'Grund', 0, 'f81d6693-8dd4-4a4c-a135-31a49f5abc62'),
-- Taco Salad
('c1000004-0001-0001-0001-000000000001', 'Kött', 0, '65e38586-6b3a-468f-a34c-f41802af246a'),
('c1000004-0001-0001-0001-000000000002', 'Sallad', 1, '65e38586-6b3a-468f-a34c-f41802af246a'),
('c1000004-0001-0001-0001-000000000003', 'Toppings', 2, '65e38586-6b3a-468f-a34c-f41802af246a'),
-- Omelette
('c1000005-0001-0001-0001-000000000001', 'Grund', 0, 'b0daf6ea-54b9-49ee-b3e9-64675c668b96');
");

            // Seed data: Ingredients
            migrationBuilder.Sql(@"
INSERT INTO ""Ingredients"" (""Id"", ""Name"", ""Calories"", ""Protein"", ""Price"", ""Volume"", ""Measure"", ""MealId"") VALUES
-- Spaghetti Bolognese
('7f6da610-34dd-450c-82ee-1a4bd421a435', 'Köttfärs', 250, 20, 5.99, 454, 'Gram', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
('4d356b92-5201-4b12-a771-1e7e86cb7c16', 'Lök', 50, 1, 0.99, 1, 'Piece', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
('855cace7-2cf3-4a75-9420-824040a31c5d', 'Vitlök', 5, 0, 0.25, 1, 'Clove', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
('4c838312-fe1b-47b1-904c-3cc20f0ffcf0', 'Tomatsås', 50, 1, 1.99, 443, 'Milliliter', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
('66b0e8e4-e452-482c-ab81-3b377ecc704e', 'Tärnade Tomater', 25, 1, 1.49, 411, 'Milliliter', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
('2238cd35-dd0f-4306-91ed-63a8eddef46f', 'Italiensk Kryddblandning', 0, 0, 0.99, 15, 'Milliliter', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
('fe05c8a1-8d6e-4aab-a5a4-13af5c3beb98', 'Spagetti', 200, 7, 1.49, 454, 'Gram', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5'),
-- Chicken Stir Fry
('9ceac1d6-b489-4cf6-a39e-712ee3f2bb0b', 'Kycklingbröst', 150, 25, 4.99, 454, 'Gram', '3ccf980c-54f2-41ba-9c56-92f9857ef58c'),
('705817fd-b188-418a-a339-cf27c56cc8c5', 'Blandade Grönsaker', 50, 2, 2.99, 473, 'Milliliter', '3ccf980c-54f2-41ba-9c56-92f9857ef58c'),
('144bbb7d-e0f4-47d6-b4b6-ccfd7c01da6c', 'Sojasås', 10, 1, 1.99, 296, 'Milliliter', '3ccf980c-54f2-41ba-9c56-92f9857ef58c'),
('4dbde69c-7228-4d41-8790-1b49b95b3974', 'Ris', 150, 3, 2.49, 473, 'Milliliter', '3ccf980c-54f2-41ba-9c56-92f9857ef58c'),
-- Grilled Cheese Sandwich
('81d5d36a-a2b3-43f5-8539-b2ac76860de2', 'Bröd', 100, 3, 1.99, 473, 'Milliliter', 'f81d6693-8dd4-4a4c-a135-31a49f5abc62'),
('73067ee5-47b5-4445-9f10-40386b7b70ed', 'Cheddarost', 110, 7, 3.99, 237, 'Milliliter', 'f81d6693-8dd4-4a4c-a135-31a49f5abc62'),
('da56d0ee-fe93-4ad9-8446-7361be30c552', 'Smör', 100, 0, 2.99, 473, 'Milliliter', 'f81d6693-8dd4-4a4c-a135-31a49f5abc62'),
-- Taco Salad
('7636ac8b-b759-4ca5-898b-0cce15cfda87', 'Köttfärs', 250, 20, 5.99, 454, 'Gram', '65e38586-6b3a-468f-a34c-f41802af246a'),
('dd59eb04-9f3c-4bff-9223-4e7f8adf9d50', 'Tacokrydda', 0, 0, 0.99, 28, 'Gram', '65e38586-6b3a-468f-a34c-f41802af246a'),
('4396c7b2-7ddf-467c-a5c7-6a6b380adfea', 'Vatten', 0, 0, 0, 237, 'Milliliter', '65e38586-6b3a-468f-a34c-f41802af246a'),
('e1dd9e66-b0c8-4733-affb-5363cc3c0333', 'Sallad', 10, 1, 1.49, 473, 'Milliliter', '65e38586-6b3a-468f-a34c-f41802af246a'),
('e546202f-ce11-48a7-a0d7-b6362a15472b', 'Tomater', 25, 1, 1.99, 473, 'Milliliter', '65e38586-6b3a-468f-a34c-f41802af246a'),
('7c7da960-521c-48c4-bef1-7cef8d8f29f7', 'Cheddarost', 110, 7, 3.99, 237, 'Milliliter', '65e38586-6b3a-468f-a34c-f41802af246a'),
('3fb19706-9c61-49de-a9c8-cb8d10d9c61c', 'Tortillachips', 140, 2, 2.49, 296, 'Milliliter', '65e38586-6b3a-468f-a34c-f41802af246a'),
('a7530fb0-1836-4830-be2b-348c8fb8138a', 'Salsa', 10, 0, 1.99, 473, 'Milliliter', '65e38586-6b3a-468f-a34c-f41802af246a'),
-- Omelette
('3f81588c-3b3e-4e50-80c4-96b4144b948d', 'Ägg', 70, 6, 1.99, 354, 'Milliliter', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96'),
('4eab134f-9264-4639-918c-80013c58cc2e', 'Smör', 100, 0, 2.99, 473, 'Milliliter', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96'),
('0a373b2d-5e60-4966-9d45-a4d0096fe198', 'Cheddarost', 110, 7, 3.99, 237, 'Milliliter', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96'),
('7ae404f1-ec40-45b8-8389-c94e8ca2c480', 'Salt', 0, 0, 0.99, 5, 'Milliliter', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96'),
('57a10701-90ef-4234-b192-487bae74cd69', 'Peppar', 0, 0, 0.99, 5, 'Milliliter', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96');
");

            // Seed data: InstructionSteps
            migrationBuilder.Sql(@"
INSERT INTO ""InstructionSteps"" (""Id"", ""StepNumber"", ""Text"", ""MealId"", ""SectionId"") VALUES
-- Spaghetti Bolognese
('a0000001-0001-0001-0001-000000000001', 1, 'Koka {0} enligt förpackningsinstruktioner.', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5', 'c1000001-0001-0001-0001-000000000003'),
('a0000001-0001-0001-0001-000000000002', 2, 'I en stor stekpanna, stek {0} över medelvärme tills den är genomstekt.', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5', 'c1000001-0001-0001-0001-000000000001'),
('a0000001-0001-0001-0001-000000000003', 3, 'Tillsätt {0} och {1} och stek tills löken är genomskinlig.', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5', 'c1000001-0001-0001-0001-000000000001'),
('a0000001-0001-0001-0001-000000000004', 4, 'Tillsätt {0}, {1} och {2}. Låt sjuda i 10 minuter.', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5', 'c1000001-0001-0001-0001-000000000002'),
('a0000001-0001-0001-0001-000000000005', 5, 'Servera såsen över spagettin.', '867fd29c-2fa3-4332-99c2-7bd5b52e64f5', NULL),
-- Chicken Stir Fry
('a0000002-0001-0001-0001-000000000001', 1, 'Koka {0} enligt förpackningsinstruktioner.', '3ccf980c-54f2-41ba-9c56-92f9857ef58c', 'c1000002-0001-0001-0001-000000000003'),
('a0000002-0001-0001-0001-000000000002', 2, 'I en stor stekpanna, stek {0} över medelvärme tills den är genomstekt.', '3ccf980c-54f2-41ba-9c56-92f9857ef58c', 'c1000002-0001-0001-0001-000000000001'),
('a0000002-0001-0001-0001-000000000003', 3, 'Tillsätt {0} och stek tills de är mjuka.', '3ccf980c-54f2-41ba-9c56-92f9857ef58c', 'c1000002-0001-0001-0001-000000000002'),
('a0000002-0001-0001-0001-000000000004', 4, 'Tillsätt {0} och rör om för att blanda.', '3ccf980c-54f2-41ba-9c56-92f9857ef58c', 'c1000002-0001-0001-0001-000000000001'),
('a0000002-0001-0001-0001-000000000005', 5, 'Servera woken över riset.', '3ccf980c-54f2-41ba-9c56-92f9857ef58c', NULL),
-- Grilled Cheese Sandwich
('a0000003-0001-0001-0001-000000000001', 1, 'Bred {0} på ena sidan av varje brödskiva.', 'f81d6693-8dd4-4a4c-a135-31a49f5abc62', 'c1000003-0001-0001-0001-000000000001'),
('a0000003-0001-0001-0001-000000000002', 2, 'Placera {0} mellan två {1} med den smörade sidan utåt.', 'f81d6693-8dd4-4a4c-a135-31a49f5abc62', 'c1000003-0001-0001-0001-000000000001'),
('a0000003-0001-0001-0001-000000000003', 3, 'Värm en stekpanna över medelvärme.', 'f81d6693-8dd4-4a4c-a135-31a49f5abc62', 'c1000003-0001-0001-0001-000000000001'),
('a0000003-0001-0001-0001-000000000004', 4, 'Lägg smörgåsen i stekpannan och stek tills brödet är gyllene och osten är smält.', 'f81d6693-8dd4-4a4c-a135-31a49f5abc62', 'c1000003-0001-0001-0001-000000000001'),
('a0000003-0001-0001-0001-000000000005', 5, 'Upprepa med resterande ingredienser för att göra fler smörgåsar.', 'f81d6693-8dd4-4a4c-a135-31a49f5abc62', 'c1000003-0001-0001-0001-000000000001'),
-- Taco Salad
('a0000004-0001-0001-0001-000000000001', 1, 'Stek {0} över medelvärme tills den är genomstekt.', '65e38586-6b3a-468f-a34c-f41802af246a', 'c1000004-0001-0001-0001-000000000001'),
('a0000004-0001-0001-0001-000000000002', 2, 'Tillsätt {0} och {1}. Låt sjuda i 10 minuter.', '65e38586-6b3a-468f-a34c-f41802af246a', 'c1000004-0001-0001-0001-000000000001'),
('a0000004-0001-0001-0001-000000000003', 3, 'I en stor skål, blanda {0}, {1}, {2} och köttfärsblandningen.', '65e38586-6b3a-468f-a34c-f41802af246a', 'c1000004-0001-0001-0001-000000000002'),
('a0000004-0001-0001-0001-000000000004', 4, 'Servera med {0} och {1}.', '65e38586-6b3a-468f-a34c-f41802af246a', 'c1000004-0001-0001-0001-000000000003'),
-- Omelette
('a0000005-0001-0001-0001-000000000001', 1, 'I en liten skål, vispa {0} med {1} och {2}.', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96', 'c1000005-0001-0001-0001-000000000001'),
('a0000005-0001-0001-0001-000000000002', 2, 'Värm en stekpanna över medelvärme.', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96', 'c1000005-0001-0001-0001-000000000001'),
('a0000005-0001-0001-0001-000000000003', 3, 'Tillsätt {0} i stekpannan och låt det smälta.', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96', 'c1000005-0001-0001-0001-000000000001'),
('a0000005-0001-0001-0001-000000000004', 4, 'Häll äggen i stekpannan och laga tills de är fasta.', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96', 'c1000005-0001-0001-0001-000000000001'),
('a0000005-0001-0001-0001-000000000005', 5, 'Tillsätt {0} och eventuella andra önskade toppings på ena halvan av omeletten.', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96', 'c1000005-0001-0001-0001-000000000001'),
('a0000005-0001-0001-0001-000000000006', 6, 'Vik den andra halvan av omeletten över toppingsen.', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96', 'c1000005-0001-0001-0001-000000000001'),
('a0000005-0001-0001-0001-000000000007', 7, 'Servera varm.', 'b0daf6ea-54b9-49ee-b3e9-64675c668b96', 'c1000005-0001-0001-0001-000000000001');
");

            // Seed data: InstructionIngredients
            migrationBuilder.Sql(@"
INSERT INTO ""InstructionIngredients"" (""Id"", ""Amount"", ""Measure"", ""SortOrder"", ""InstructionStepId"", ""IngredientId"", ""SectionId"") VALUES
-- Spaghetti Bolognese
('b0000001-0001-0001-0001-000000000001', 454, 'Gram', 0, 'a0000001-0001-0001-0001-000000000001', 'fe05c8a1-8d6e-4aab-a5a4-13af5c3beb98', 'c1000001-0001-0001-0001-000000000003'),
('b0000001-0001-0001-0001-000000000002', 454, 'Gram', 0, 'a0000001-0001-0001-0001-000000000002', '7f6da610-34dd-450c-82ee-1a4bd421a435', 'c1000001-0001-0001-0001-000000000001'),
('b0000001-0001-0001-0001-000000000003', 1, 'Piece', 0, 'a0000001-0001-0001-0001-000000000003', '4d356b92-5201-4b12-a771-1e7e86cb7c16', 'c1000001-0001-0001-0001-000000000001'),
('b0000001-0001-0001-0001-000000000004', 1, 'Clove', 1, 'a0000001-0001-0001-0001-000000000003', '855cace7-2cf3-4a75-9420-824040a31c5d', 'c1000001-0001-0001-0001-000000000002'),
('b0000001-0001-0001-0001-000000000005', 443, 'Milliliter', 0, 'a0000001-0001-0001-0001-000000000004', '4c838312-fe1b-47b1-904c-3cc20f0ffcf0', 'c1000001-0001-0001-0001-000000000002'),
('b0000001-0001-0001-0001-000000000006', 411, 'Milliliter', 1, 'a0000001-0001-0001-0001-000000000004', '66b0e8e4-e452-482c-ab81-3b377ecc704e', 'c1000001-0001-0001-0001-000000000002'),
('b0000001-0001-0001-0001-000000000007', 15, 'Milliliter', 2, 'a0000001-0001-0001-0001-000000000004', '2238cd35-dd0f-4306-91ed-63a8eddef46f', 'c1000001-0001-0001-0001-000000000002'),
-- Chicken Stir Fry
('b0000002-0001-0001-0001-000000000001', 473, 'Milliliter', 0, 'a0000002-0001-0001-0001-000000000001', '4dbde69c-7228-4d41-8790-1b49b95b3974', 'c1000002-0001-0001-0001-000000000003'),
('b0000002-0001-0001-0001-000000000002', 454, 'Gram', 0, 'a0000002-0001-0001-0001-000000000002', '9ceac1d6-b489-4cf6-a39e-712ee3f2bb0b', 'c1000002-0001-0001-0001-000000000001'),
('b0000002-0001-0001-0001-000000000003', 473, 'Milliliter', 0, 'a0000002-0001-0001-0001-000000000003', '705817fd-b188-418a-a339-cf27c56cc8c5', 'c1000002-0001-0001-0001-000000000002'),
('b0000002-0001-0001-0001-000000000004', 296, 'Milliliter', 0, 'a0000002-0001-0001-0001-000000000004', '144bbb7d-e0f4-47d6-b4b6-ccfd7c01da6c', 'c1000002-0001-0001-0001-000000000001'),
-- Grilled Cheese Sandwich
('b0000003-0001-0001-0001-000000000001', 473, 'Milliliter', 0, 'a0000003-0001-0001-0001-000000000001', 'da56d0ee-fe93-4ad9-8446-7361be30c552', 'c1000003-0001-0001-0001-000000000001'),
('b0000003-0001-0001-0001-000000000002', 237, 'Milliliter', 0, 'a0000003-0001-0001-0001-000000000002', '73067ee5-47b5-4445-9f10-40386b7b70ed', 'c1000003-0001-0001-0001-000000000001'),
('b0000003-0001-0001-0001-000000000003', 473, 'Milliliter', 1, 'a0000003-0001-0001-0001-000000000002', '81d5d36a-a2b3-43f5-8539-b2ac76860de2', 'c1000003-0001-0001-0001-000000000001'),
-- Taco Salad
('b0000004-0001-0001-0001-000000000001', 454, 'Gram', 0, 'a0000004-0001-0001-0001-000000000001', '7636ac8b-b759-4ca5-898b-0cce15cfda87', 'c1000004-0001-0001-0001-000000000001'),
('b0000004-0001-0001-0001-000000000002', 28, 'Gram', 0, 'a0000004-0001-0001-0001-000000000002', 'dd59eb04-9f3c-4bff-9223-4e7f8adf9d50', 'c1000004-0001-0001-0001-000000000001'),
('b0000004-0001-0001-0001-000000000003', 237, 'Milliliter', 1, 'a0000004-0001-0001-0001-000000000002', '4396c7b2-7ddf-467c-a5c7-6a6b380adfea', 'c1000004-0001-0001-0001-000000000001'),
('b0000004-0001-0001-0001-000000000004', 473, 'Milliliter', 0, 'a0000004-0001-0001-0001-000000000003', 'e1dd9e66-b0c8-4733-affb-5363cc3c0333', 'c1000004-0001-0001-0001-000000000002'),
('b0000004-0001-0001-0001-000000000005', 473, 'Milliliter', 1, 'a0000004-0001-0001-0001-000000000003', 'e546202f-ce11-48a7-a0d7-b6362a15472b', 'c1000004-0001-0001-0001-000000000002'),
('b0000004-0001-0001-0001-000000000006', 237, 'Milliliter', 2, 'a0000004-0001-0001-0001-000000000003', '7c7da960-521c-48c4-bef1-7cef8d8f29f7', 'c1000004-0001-0001-0001-000000000003'),
('b0000004-0001-0001-0001-000000000007', 296, 'Milliliter', 0, 'a0000004-0001-0001-0001-000000000004', '3fb19706-9c61-49de-a9c8-cb8d10d9c61c', 'c1000004-0001-0001-0001-000000000003'),
('b0000004-0001-0001-0001-000000000008', 473, 'Milliliter', 1, 'a0000004-0001-0001-0001-000000000004', 'a7530fb0-1836-4830-be2b-348c8fb8138a', 'c1000004-0001-0001-0001-000000000003'),
-- Omelette
('b0000005-0001-0001-0001-000000000001', 354, 'Milliliter', 0, 'a0000005-0001-0001-0001-000000000001', '3f81588c-3b3e-4e50-80c4-96b4144b948d', 'c1000005-0001-0001-0001-000000000001'),
('b0000005-0001-0001-0001-000000000002', 5, 'Milliliter', 1, 'a0000005-0001-0001-0001-000000000001', '7ae404f1-ec40-45b8-8389-c94e8ca2c480', 'c1000005-0001-0001-0001-000000000001'),
('b0000005-0001-0001-0001-000000000003', 5, 'Milliliter', 2, 'a0000005-0001-0001-0001-000000000001', '57a10701-90ef-4234-b192-487bae74cd69', 'c1000005-0001-0001-0001-000000000001'),
('b0000005-0001-0001-0001-000000000004', 473, 'Milliliter', 0, 'a0000005-0001-0001-0001-000000000003', '4eab134f-9264-4639-918c-80013c58cc2e', 'c1000005-0001-0001-0001-000000000001'),
('b0000005-0001-0001-0001-000000000005', 237, 'Milliliter', 0, 'a0000005-0001-0001-0001-000000000005', '0a373b2d-5e60-4966-9d45-a4d0096fe198', 'c1000005-0001-0001-0001-000000000001');
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seed data (cascade deletes sections, ingredients, instruction steps, and instruction ingredients)
            migrationBuilder.Sql(@"
DELETE FROM ""Meals"" WHERE ""Id"" IN (
    '867fd29c-2fa3-4332-99c2-7bd5b52e64f5',
    '3ccf980c-54f2-41ba-9c56-92f9857ef58c',
    'f81d6693-8dd4-4a4c-a135-31a49f5abc62',
    '65e38586-6b3a-468f-a34c-f41802af246a',
    'b0daf6ea-54b9-49ee-b3e9-64675c668b96');
");

            migrationBuilder.DropTable(
                name: "InstructionIngredients");

            migrationBuilder.DropTable(
                name: "InstructionSteps");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.AddColumn<List<string>>(
                name: "Instructions",
                table: "Meals",
                type: "text[]",
                nullable: false);
        }
    }
}
