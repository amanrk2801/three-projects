package com.example.ecommerce.controller;

import com.example.ecommerce.entity.CartItem;
import com.example.ecommerce.entity.Product;
import com.example.ecommerce.entity.User;
import com.example.ecommerce.repository.CartItemRepository;
import com.example.ecommerce.repository.ProductRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;

@CrossOrigin(origins = "*", maxAge = 3600)
@RestController
@RequestMapping("/api/cart")
public class CartController {
    
    @Autowired
    private CartItemRepository cartItemRepository;
    
    @Autowired
    private ProductRepository productRepository;
    
    @GetMapping
    public ResponseEntity<Map<String, Object>> getCart(Authentication authentication) {
        User user = (User) authentication.getPrincipal();
        List<CartItem> cartItems = cartItemRepository.findByUser(user);
        
        Double total = cartItemRepository.getCartTotalByUser(user);
        if (total == null) total = 0.0;
        
        Map<String, Object> response = new HashMap<>();
        response.put("items", cartItems);
        response.put("total", total);
        response.put("itemCount", cartItems.size());
        
        return ResponseEntity.ok(response);
    }
    
    @PostMapping("/add")
    public ResponseEntity<?> addToCart(@RequestBody Map<String, Object> request, Authentication authentication) {
        User user = (User) authentication.getPrincipal();
        Long productId = Long.valueOf(request.get("productId").toString());
        Integer quantity = Integer.valueOf(request.get("quantity").toString());
        
        Optional<Product> optionalProduct = productRepository.findById(productId);
        if (!optionalProduct.isPresent() || !optionalProduct.get().getActive()) {
            Map<String, String> response = new HashMap<>();
            response.put("message", "Product not found");
            return ResponseEntity.badRequest().body(response);
        }
        
        Product product = optionalProduct.get();
        
        if (!product.hasStock(quantity)) {
            Map<String, String> response = new HashMap<>();
            response.put("message", "Insufficient stock");
            return ResponseEntity.badRequest().body(response);
        }
        
        Optional<CartItem> existingCartItem = cartItemRepository.findByUserAndProduct(user, product);
        
        if (existingCartItem.isPresent()) {
            CartItem cartItem = existingCartItem.get();
            int newQuantity = cartItem.getQuantity() + quantity;
            
            if (!product.hasStock(newQuantity)) {
                Map<String, String> response = new HashMap<>();
                response.put("message", "Insufficient stock for requested quantity");
                return ResponseEntity.badRequest().body(response);
            }
            
            cartItem.setQuantity(newQuantity);
            cartItemRepository.save(cartItem);
        } else {
            CartItem cartItem = new CartItem(user, product, quantity);
            cartItemRepository.save(cartItem);
        }
        
        Map<String, String> response = new HashMap<>();
        response.put("message", "Product added to cart successfully");
        return ResponseEntity.ok(response);
    }
    
    @PutMapping("/update/{itemId}")
    public ResponseEntity<?> updateCartItem(@PathVariable Long itemId, @RequestBody Map<String, Integer> request, 
                                          Authentication authentication) {
        User user = (User) authentication.getPrincipal();
        Integer quantity = request.get("quantity");
        
        Optional<CartItem> optionalCartItem = cartItemRepository.findById(itemId);
        if (!optionalCartItem.isPresent()) {
            Map<String, String> response = new HashMap<>();
            response.put("message", "Cart item not found");
            return ResponseEntity.notFound().build();
        }
        
        CartItem cartItem = optionalCartItem.get();
        
        // Check if the cart item belongs to the authenticated user
        if (!cartItem.getUser().getId().equals(user.getId())) {
            Map<String, String> response = new HashMap<>();
            response.put("message", "Unauthorized");
            return ResponseEntity.status(403).body(response);
        }
        
        if (quantity <= 0) {
            cartItemRepository.delete(cartItem);
            Map<String, String> response = new HashMap<>();
            response.put("message", "Item removed from cart");
            return ResponseEntity.ok(response);
        }
        
        if (!cartItem.getProduct().hasStock(quantity)) {
            Map<String, String> response = new HashMap<>();
            response.put("message", "Insufficient stock");
            return ResponseEntity.badRequest().body(response);
        }
        
        cartItem.setQuantity(quantity);
        cartItemRepository.save(cartItem);
        
        Map<String, String> response = new HashMap<>();
        response.put("message", "Cart updated successfully");
        return ResponseEntity.ok(response);
    }
    
    @DeleteMapping("/remove/{itemId}")
    public ResponseEntity<?> removeFromCart(@PathVariable Long itemId, Authentication authentication) {
        User user = (User) authentication.getPrincipal();
        
        Optional<CartItem> optionalCartItem = cartItemRepository.findById(itemId);
        if (!optionalCartItem.isPresent()) {
            return ResponseEntity.notFound().build();
        }
        
        CartItem cartItem = optionalCartItem.get();
        
        // Check if the cart item belongs to the authenticated user
        if (!cartItem.getUser().getId().equals(user.getId())) {
            Map<String, String> response = new HashMap<>();
            response.put("message", "Unauthorized");
            return ResponseEntity.status(403).body(response);
        }
        
        cartItemRepository.delete(cartItem);
        
        Map<String, String> response = new HashMap<>();
        response.put("message", "Item removed from cart successfully");
        return ResponseEntity.ok(response);
    }
    
    @DeleteMapping("/clear")
    public ResponseEntity<?> clearCart(Authentication authentication) {
        User user = (User) authentication.getPrincipal();
        cartItemRepository.deleteByUser(user);
        
        Map<String, String> response = new HashMap<>();
        response.put("message", "Cart cleared successfully");
        return ResponseEntity.ok(response);
    }
}