# Unit Test Documentation

## Purpose
These unit tests validate the functionality of the `ProductService` clas.

## Tests Overview

### `Service Method Coverage: GetAllProducts`
#### GetAllProducts_ReturnsProductDtos
##### **Purpose**: 
- Verify the number of items returned by `GetAllProducts` matches the number of items in the original list (products.Count).
- Verify properties of returned `ProductDto`s match those of the entities in the list.
- Test the behavior when no products exist (returning an empty list).

---

### `Service Method Coverage: GetProductsByName`
#### GetProductsByName_ReturnsFilteredProductDtos
##### **Purpose**:
- Verify that `GetProductsByName` correctly filters products based on the provided `name`.
- Verify the number of filtered products matches the expected count.
- Verify properties of filtered `ProductDto`s match the `Product` entity.

#### GetProductsByName_ReturnsEmptyList_WhenNoProductsMatch
##### **Purpose**:
- Test the behavior when no products match the provided name (returning an empty list).

---

### `Service Method Coverage: GetProductById`
#### GetProductById_ReturnsProductDto_IfExists
##### **Purpose**:
- Verify that `GetProductById` returns a `ProductDto` when a product with the given `id` exists.
- Verify that the returned `ProductDto` matches the entity properties.

#### GetProductById_ReturnsNull_IfProductDoesNotExist
##### **Purpose**:
- Test that `GetProductById` returns `null` when a product with the given `id` does not exist.

---

### `Service Method Coverage: CreateProduct`
#### CreateProduct_AddsProductToDatabase_ReturnsCreatedProductDto
##### **Purpose**:
- Verify that `CreateProduct` adds a new `Product` to the database.
- Verify that the returned `ProductDto` contains the correct details, including the generated `Id`.

---



### `Service Method Coverage: UpdateProduct`
#### UpdateProduct_ReturnsTrue_WhenProductExists
##### **Purpose**:
- Verify that `UpdateProduct` returns `true` when the product with the given `id` exists.
- Verify that the product fields are updated correctly.

#### UpdateProduct_ReturnsFalse_WhenProductDoesNotExist
##### **Purpose**:
- Test that `UpdateProduct` returns `false` when a product with the given `id` does not exist.

---


### `Service Method Coverage: GetOptionsByProductId`

#### GetOptionsByProductId_ReturnsOptions_WhenOptionsExist
##### **Purpose**:
- Verify that `GetOptionsByProductId` returns a list of `ProductOptionDto` when options associated with the given `productId` exist.
- Ensure that the number of returned options matches the number of existing options.
- Verify that the properties of the returned `ProductOptionDto` match the entity properties (e.g., `Name`).


#### GetOptionsByProductId_ReturnsEmptyList_WhenNoOptionsExist
##### **Purpose**:
- Test that `GetOptionsByProductId` returns an empty list when no `ProductOption` entities exist for the given `productId`.
- Ensure that the result is not null and is an empty list when no options are found.

---


### `Service Method Coverage: GetOptionByProductIdAndOptionId`

#### GetOptionByProductIdAndOptionId_ReturnsOption_WhenOptionExists
##### **Purpose**:
- Verify that `GetOptionByProductIdAndOptionId` returns a `ProductOptionDto` when both the `productId` and `optionId` exist and the option belongs to the specified product.
- Ensure that the returned `ProductOptionDto` matches the entity properties, such as `Name` and `Description`.

#### GetOptionByProductIdAndOptionId_ReturnsNull_WhenProductDoesNotExist
##### **Purpose**:
- Test that `GetOptionByProductIdAndOptionId` returns `null` if no product with the given `productId` exists in the database.

#### GetOptionByProductIdAndOptionId_ReturnsNull_WhenOptionDoesNotExist
##### **Purpose**:
- Verify that `GetOptionByProductIdAndOptionId` returns `null` if the option with the given `optionId` does not exist, or if the option exists but does not belong to the product with the specified `productId`.

---
### `Service Method Coverage: DoesProductExist`

#### DoesProductExist_ReturnsTrue_WhenProductExists
##### **Purpose**:
- Verify that `DoesProductExist` returns `true` when a product with the given `productId` exists in the database.

#### DoesProductExist_ReturnsFalse_WhenProductDoesNotExist
##### **Purpose**:
- Verify that `DoesProductExist` returns `false` when no product with the given `productId` exists in the database.

---
### `Service Method Coverage: AddOptionToProduct`

#### AddOptionToProduct_AddsNewOption_WhenProductExists
##### **Purpose**:
- Verify that `AddOptionToProduct` successfully adds a new option to a product when the product exists.
- Ensure that the returned `ProductOptionDto` matches the newly added option's properties.
- Verify that the `ProductOptions.AddAsync` and `SaveChangesAsync` methods are called exactly once.

#### AddOptionToProduct_ThrowsException_WhenProductDoesNotExist
##### **Purpose**:
- Verify that `AddOptionToProduct` throws an exception when trying to add an option to a non-existent product.
- Ensure that the exception message matches "Product not found."

---
### `Service Method Coverage: UpdateProductOption`

#### UpdateProductOption_ReturnsTrue_WhenOptionExists
##### **Purpose**:
- Verify that `UpdateProductOption` returns `true` when a matching product option with the given `productId` and `optionId` exists.
- Ensure that the option's properties (`Name` and `Description`) are correctly updated.


#### UpdateProductOption_ReturnsFalse_WhenOptionDoesNotExist
##### **Purpose**:
- Verify that `UpdateProductOption` returns `false` when no matching product option with the given `productId` and `optionId` exists.


### `Service Method Coverage: DeleteProductOption`

#### DeleteProductOption_ReturnsTrue_WhenOptionExists
##### **Purpose**:
- Verify that `DeleteProductOption` returns `true` when a product option with the given `productId` and `optionId` exists.
- Ensure that the `Remove` method is called to delete the option.
- Confirm that `SaveChangesAsync` is called once to persist the deletion.

#### DeleteProductOption_ReturnsFalse_WhenOptionDoesNotExist
##### **Purpose**:
- Verify that `DeleteProductOption` returns `false` when no matching product option with the given `productId` and `optionId` exists.
- Ensure that `Remove` is never called if the option is not found.
- Confirm that `SaveChangesAsync` is not called if no deletion is performed.
