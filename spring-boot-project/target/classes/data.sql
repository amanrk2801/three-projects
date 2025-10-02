-- Insert sample users (password is 'password123' encoded with BCrypt)
INSERT INTO users (name, email, password, role, created_at, updated_at) VALUES 
('Admin User', 'admin@example.com', '$2a$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2uheWG/igi.', 'ADMIN', NOW(), NOW()),
('John Doe', 'john@example.com', '$2a$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2uheWG/igi.', 'CUSTOMER', NOW(), NOW()),
('Jane Smith', 'jane@example.com', '$2a$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2uheWG/igi.', 'CUSTOMER', NOW(), NOW());

-- Insert sample products
INSERT INTO products (name, description, price, stock_quantity, category, image_url, active, created_at, updated_at) VALUES 
('iPhone 14 Pro', 'Latest Apple smartphone with advanced camera system', 999.99, 50, 'Electronics', 'https://example.com/iphone14.jpg', true, NOW(), NOW()),
('Samsung Galaxy S23', 'Flagship Android smartphone with excellent display', 899.99, 30, 'Electronics', 'https://example.com/galaxy-s23.jpg', true, NOW(), NOW()),
('MacBook Air M2', 'Lightweight laptop with Apple M2 chip', 1199.99, 25, 'Electronics', 'https://example.com/macbook-air.jpg', true, NOW(), NOW()),
('Dell XPS 13', 'Premium Windows laptop with InfinityEdge display', 999.99, 20, 'Electronics', 'https://example.com/dell-xps13.jpg', true, NOW(), NOW()),
('Sony WH-1000XM4', 'Wireless noise-canceling headphones', 349.99, 100, 'Electronics', 'https://example.com/sony-headphones.jpg', true, NOW(), NOW()),
('Nike Air Max 270', 'Comfortable running shoes with Air Max technology', 150.00, 75, 'Footwear', 'https://example.com/nike-airmax.jpg', true, NOW(), NOW()),
('Adidas Ultraboost 22', 'High-performance running shoes with Boost midsole', 180.00, 60, 'Footwear', 'https://example.com/adidas-ultraboost.jpg', true, NOW(), NOW()),
('Levi''s 501 Jeans', 'Classic straight-fit denim jeans', 89.99, 120, 'Clothing', 'https://example.com/levis-501.jpg', true, NOW(), NOW()),
('Champion Hoodie', 'Comfortable cotton blend hoodie', 45.99, 80, 'Clothing', 'https://example.com/champion-hoodie.jpg', true, NOW(), NOW()),
('The Great Gatsby', 'Classic American novel by F. Scott Fitzgerald', 12.99, 200, 'Books', 'https://example.com/great-gatsby.jpg', true, NOW(), NOW()),
('To Kill a Mockingbird', 'Pulitzer Prize-winning novel by Harper Lee', 14.99, 150, 'Books', 'https://example.com/mockingbird.jpg', true, NOW(), NOW()),
('1984', 'Dystopian novel by George Orwell', 13.99, 180, 'Books', 'https://example.com/1984.jpg', true, NOW(), NOW());

-- Insert sample cart items
INSERT INTO cart_items (user_id, product_id, quantity, added_at) VALUES 
(2, 1, 1, NOW()),
(2, 5, 2, NOW()),
(3, 3, 1, NOW()),
(3, 6, 1, NOW());

-- Insert sample orders
INSERT INTO orders (user_id, total_amount, status, shipping_address, order_date) VALUES 
(2, 1349.98, 'DELIVERED', '123 Main St, New York, NY 10001', '2024-09-15 10:30:00'),
(3, 1199.99, 'SHIPPED', '456 Oak Ave, Los Angeles, CA 90210', '2024-09-20 14:15:00'),
(2, 45.99, 'PENDING', '123 Main St, New York, NY 10001', '2024-09-25 09:45:00');

-- Insert sample order items
INSERT INTO order_items (order_id, product_id, quantity, price) VALUES 
(1, 1, 1, 999.99),
(1, 5, 1, 349.99),
(2, 3, 1, 1199.99),
(3, 9, 1, 45.99);