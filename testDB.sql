use MuseumTicketsDB;

INSERT INTO MuseumComplex (Name) 
VALUES (N'Музейное Объединение ВлГУ');

INSERT INTO Museum (MuseumComplexID, Name, City, Street, House) 
VALUES (10001, N'Музей Истории', N'Владимир', N'Мира', '25');

INSERT INTO Exhibition (Name, Photo, StartDate, EndDate) 
VALUES (N'Золото предков', '/images/gold.jpg', '2024-03-01', '2024-12-31');

INSERT INTO MuseumExhibition (MuseumID, ExhibitionID) 
VALUES (10001, 10001);

INSERT INTO Ticket (ExhibitionID, Type, Status, AvailableQuantity) 
VALUES (10001, N'Взрослый', N'Доступен', 100);

INSERT INTO TicketPrice (TicketID, Price) 
VALUES (10001, 550.00);

SELECT 
    e.Name AS Exhibition, 
    t.Type AS TicketType, 
    tp.Price, 
    t.AvailableQuantity,
    e.Photo
FROM Exhibition e
JOIN Ticket t ON e.ExhibitionID = t.ExhibitionID
JOIN TicketPrice tp ON t.TicketID = tp.TicketID;
