package com.example.ecommerce.repository;

import com.example.ecommerce.entity.CartItem;
import com.example.ecommerce.entity.Product;
import com.example.ecommerce.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface CartItemRepository extends JpaRepository<CartItem, Long> {
    
    List<CartItem> findByUser(User user);
    
    Optional<CartItem> findByUserAndProduct(User user, Product product);
    
    void deleteByUser(User user);
    
    @Query("SELECT SUM(ci.quantity * ci.product.price) FROM CartItem ci WHERE ci.user = :user")
    Double getCartTotalByUser(@Param("user") User user);
    
    @Query("SELECT COUNT(ci) FROM CartItem ci WHERE ci.user = :user")
    Long getCartItemCountByUser(@Param("user") User user);
}