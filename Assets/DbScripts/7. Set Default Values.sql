INSERT INTO main."Currencies" ("Name","Sumbol","OtherNames","IsDefault","IsMainStable","LastExchangeRate") 
VALUES ('Доллары', '$', 'Бакс, доллары, долларов, dollars, doll, зелень', 0, 1, 1)

INSERT INTO main."Currencies" ("Name","Sumbol","OtherNames","IsDefault","IsMainStable","LastExchangeRate") 
VALUES ('Драм', '֏', 'драм, копейки, фантик, dram, драмм', 1, 0, 395.0)

INSERT INTO main."LimitTypes" ("Name", "Period", "StartPeriod","IsAutoProlongation","IsActive") 
VALUES ('Custom', '00.00.00', '30.08.2023', 0, 1)

INSERT INTO main."Users" (TName, RealName, DateCreate) VALUES
('@Admin', 'Creator', '30.08.2023')

INSERT INTO main."Categories" (	ParentID,	Name,	CreatedBy, CreateDate) 
VALUES (NULL, 'Default', 1, '30.08.2023')