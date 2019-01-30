/*
Create the database from scratch
(Delete it if already exists).
*/
DROP DATABASE IF EXISTS alexshop;
CREATE DATABASE alexshop;

USE alexshop;

/*
Create products table.
*/
CREATE TABLE products
(
	id INT PRIMARY KEY AUTO_INCREMENT,
    price DECIMAL(6, 2) NOT NULL,
    product_name CHAR(30) UNIQUE NOT NULL,
    supplier CHAR(30) DEFAULT '',
    phone CHAR(10) DEFAULT '',
    barcode CHAR(255) UNIQUE NOT NULL,
    quantity INT DEFAULT 0,
    sold_quantity INT DEFAULT 0,
    descrip TEXT
);



/*
Create sold table.
This table keep's records of
each product that has been sold.
*/
CREATE TABLE sold
(
	product_id INT NOT NULL,
    date_out DATE NOT NULL,
    FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);



/*
Create add_product PROCEDURE.
This procedure insert's a product.
*/
DELIMITER $$
CREATE PROCEDURE add_product(IN price DECIMAL(6,2), IN pr_name CHAR(30), IN sup CHAR(30),
IN phon CHAR(10), IN des TEXT, IN barc CHAR(255))
	BEGIN
		INSERT INTO products (price, product_name, supplier, phone, descrip, barcode)
			   VALUES (price, pr_name, sup, phon, des, barc);
	END$$
DELIMITER ;




/*
Create delete_product PROCEDURE.
This procedure delete's a product.
*/
DELIMITER $$
CREATE PROCEDURE delete_product(IN prod_name CHAR(30))
	BEGIN
		DELETE FROM products
        WHERE product_name = prod_name;
	END$$
DELIMITER ;



/*
Create add_quantity PROCEDURE.
This procedure adds an amount
of quantity into a specified product
based on the barcode.
*/
DELIMITER $$
CREATE PROCEDURE add_quantity(IN barc CHAR(255), IN amount INT)
	BEGIN
		-- If the item does not exist.
        IF NOT EXISTS (SELECT * FROM products WHERE barcode = barc) THEN
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Not Found';
            
            
		ELSE
			UPDATE products
			SET quantity = quantity + amount
			WHERE barcode = barc;
		END IF;
	END$$
DELIMITER ;



/*
Create add_sold_item Procedure.
This procedure adds a sold item
and updates the product status.
*/
DELIMITER $$
CREATE PROCEDURE add_sold_item(IN barc CHAR(255))
	BEGIN
		
        -- If the item does not exist.
        IF NOT EXISTS (SELECT * FROM products WHERE barcode = barc) THEN
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Not Found';
            
		
        -- Quantity is zero.
		ELSEIF (SELECT quantity FROM products WHERE barcode = barc) = 0 THEN
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Zero Quantity';
		
        
        -- Else
		ELSE
        
			-- Insert a sold item.
			INSERT INTO sold
				SELECT id, now()
				FROM products
				WHERE barcode = barc;
            
            -- Decrease the quantity of the product
            -- and Increase the sold quantity.
			UPDATE products
			SET quantity  = quantity-1, sold_quantity = sold_quantity+1
			WHERE barcode = barc;
		END IF;
	END$$
DELIMITER ;



/*
Create update_product Procedure.
This procedure updates all the values
of a single product.
*/
DELIMITER $$
CREATE PROCEDURE update_product(IN pr_id INT, IN pri DECIMAL(6,2), IN pr_name CHAR(30), IN sup CHAR(30),
IN phon CHAR(10), IN des TEXT, IN barc CHAR(255))
	BEGIN
		UPDATE products
        SET price = pri, product_name = pr_name, supplier = sup, phone = phon, descrip = des, barcode = barc
        WHERE id  = pr_id;
	END$$
DELIMITER ;



/*
Create get_analysis Procedure.
This procedure return's all the product
records of a specified date.
*/
DELIMITER $$
CREATE PROCEDURE get_analysis(IN year_out INT, IN month_out INT)
	BEGIN
    
		
        -- Return all the sold items.
		IF year_out IS NULL AND month_out IS NULL THEN
			SELECT *
			FROM products NATURAL JOIN
							(SELECT product_id AS id, date_out
							 FROM sold
							)
							 AS sold_table;
		
        
        -- Return sold items grouped by id with year filtering.
		ELSEIF month_out IS NULL THEN
			SELECT product_name, price, date_out, COUNT(id) as amount
			FROM products NATURAL JOIN
							(SELECT product_id AS id, date_out
							 FROM sold
							 WHERE YEAR(date_out) = year_out
							)
							 AS sold_table
			GROUP BY id;
        
        
        -- Return sold items grouped by id with year and month filtering.
        ELSE
			SELECT product_name, price, date_out, COUNT(id) as amount
			FROM products NATURAL JOIN
							(SELECT product_id AS id, date_out
							 FROM sold
							 WHERE YEAR(date_out) = year_out 
							 AND MONTH(date_out)  = month_out
							)
							 AS sold_table
			GROUP BY id;
        END IF;
		
	END$$
DELIMITER ;



/*
Create get_analysis_dates Procedure.
This procedure return's all the sold
item dates.
*/
DELIMITER $$
CREATE PROCEDURE get_analysis_dates()
	BEGIN
		SELECT date_out
        FROM sold
        ORDER BY date_out;
	END$$
DELIMITER ;