CREATE DATABASE TvShowsDB;

USE TvShowsDB;

CREATE TABLE TvShows (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Favorite BIT NOT NULL
);

INSERT INTO TvShows (Name, Favorite) VALUES
('El Chavo del 8', 1),
('Cuna de Lobos', 1),
('La Casa de las Flores', 1),
('Rebelde', 1),
('La Rosa de Guadalupe', 1),
('El Juego de las Llaves', 1),
('Por siempre Joan Sebastian', 1),
('El Señor de los Cielos', 1),
('Club de Cuervos', 1),
('Sordo', 1),
('El Recluso', 1),
('Los Miserables', 1),
('Las Aparicio', 1),
('Soy Tu Fan', 1),
('La Malquerida', 1),
('Hasta que te conocí', 1),
('Cuando calla el cantor', 1),
('Mujeres Asesinas', 1),
('Café con Aroma de Mujer', 1),
('La fea más bella', 1),
('Vivir sin ti', 1),
('Como dice el dicho', 1),
('Amor de mis amores', 1),
('Amor Eterno', 1),
('Los 10 de México', 1);


CREATE PROCEDURE sp_listTvShows
    @Search VARCHAR(50) = '',
    @PageNumber INT = 1,
    @PageSize INT = 10,
	@IsFavorite BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        tvs.Id,
        tvs.Name,
        tvs.Favorite
    FROM 
        TvShows tvs
    WHERE 
        tvs.Name LIKE '%' + @Search + '%'
		AND (@IsFavorite IS NULL OR tvs.Favorite = @IsFavorite)
    ORDER BY 
        tvs.Id DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END

CREATE PROCEDURE sp_addTvShow
    @Name VARCHAR(50),
    @Favorite BIT,
    @Msg VARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
		IF(EXISTS(SELECT * FROM TvShows WHERE Name = @Name))
		BEGIN
			SET @Msg = 'Conflict'
			RETURN;
		END

        INSERT INTO TvShows (Name, Favorite)
        VALUES (@Name, @Favorite);

        SET @Msg = 'Ok';
    END TRY
    BEGIN CATCH
        SET @Msg = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END

CREATE PROCEDURE sp_updateTvShow
    @Id INT,
    @Name VARCHAR(50),
    @Favorite BIT,
    @Msg VARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS(SELECT * FROM TvShows WHERE Id = @Id)
        BEGIN
            SET @Msg = 'NotFound';
            RETURN;
        END

        IF EXISTS(SELECT * FROM TvShows WHERE Name = @Name AND Id != @Id)
        BEGIN
            SET @Msg = 'Conflict';
            RETURN;
        END

        UPDATE TvShows
        SET Name = @Name,
            Favorite = @Favorite
        WHERE Id = @Id;

        SET @Msg = 'Ok';
    END TRY
    BEGIN CATCH
        SET @Msg = 'Error: ' + ERROR_MESSAGE(); 
    END CATCH
END


CREATE PROCEDURE sp_deleteTvShow
    @Id INT,
    @Msg VARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
		IF NOT EXISTS(SELECT * FROM TvShows WHERE Id = @Id)
        BEGIN
            SET @Msg = 'NotFound';
            RETURN;
        END

        DELETE FROM TvShows
        WHERE Id = @Id;

        SET @Msg = 'Ok';
    END TRY
    BEGIN CATCH
        SET @Msg = 'Error: ' + ERROR_MESSAGE(); 
    END CATCH
END