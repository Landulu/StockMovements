Hello dear developper!

In this exercise you have to write a class that allows the management of stock of products.


=================================
Modelization
=================================

A stock relates to several products.
A product is identified by an EAN (8 alphanumeric characters)
A movement (or flow) of products contains a date, a description, and one or more sets (product, quantity).


=================================
General features
=================================
We must be able for this stock:
 - to add quantities of products (for example when purchasing from suppliers)
 - to delete quantities of products (for example when ordering customers)
 - to add several stock movements on several products on a date (but with a single caption)
 - to know the stock of a product at any date in the past
 - to know the stock variations of a product during any period (interval of dates)
   If a stock is at 10 on a date D1, and at 20 on a date D2, then the change between D1 and D2 is +10.
 - to know the current stock of a product
 - to know the products currently in stock (negative stocks are considered null)
 - to know the total number of products in the stock (negative stocks are considered as zero)
 - to regularize the stock of a product (see inventory)

The movements do not necessarily follow each other over time,
but it is not possible to add a movement at a date earlier than or equal to that of an inventory.
(a movement before an inventory is not possible)


===============================
Inventory
================================
From time to time a stock regulation (an inventory) is made on a product.
We must then be able to create a movement for a product which fixes the stock at a given value on today's date.


===============================
Details
===============================
* Adding a non-existing product to the stock creates the product.
* It is possible to have negative quantities.
* Inventory movements are unique and cannot be negative.
* If, when inserting a set of movements, one of the values ​​is not authorized (due to subsequent inventory),
  all operations are rejected.
* Movement times are ignored.
* When the stock is created, no product is present.


=================================
Implementation
=================================
Your implementation must contain at least one stock management class, and one product class.
You are not limited to these two classes, you can create more if necessary.
The stock management class must contain at least the following operations:
 - Unique movement for a product on a date
 - Movement for one or more product on a date
 - Inventory movement for a product
 - Recovery of the value of the stock on a date
 - Recovery of stock variation over a date interval
 - List of products in stock

Here is an example of a possible signature of the addition of a movement on a product.
public void SetRealFunctionName(DateTime date, string label, Product product, int quantity)

You don't have to add any third-party nuggets to your business library (but you can add some to your test library)


=================================
Performance
=================================
The system is considered fast enough that it does not have to store calculated values.
However, you should not do more calculation than necessary.


=================================
Example
=================================

for the following operations:

+ ------------ + ------------ + -------- + ---------- +
| Date         | Wording      | Quantity | product    |
+ ------------ + ------------ + -------- + ---------- +
| 01/01/2020   | Purchase N°1 | 10       | EAN00001   |
| 01/01/2020   | Purchase N°2 | 10       | EAN00002   |
| 01/01/2020   | Purchase N°3 | 10       | EAN00003   |
| 01/02/2020   | Order N°1    | -3       | EAN00001   |
| 01/02/2020   | Order N°1    | -3       | EAN00002   |
| 01/02/2020   | Order N°1    | -3       | EAN00003   |
| 01/03/2020   | Order N°2    | -1       | EAN00001   |
| 01/03/2020   | Order N°2    | -10      | EAN00002   |
| 01/04/2020   | inventory    | 1        | EAN00002   |
+ ------------ + ------------ + -------- + ---------- +


we will get the following stock values:

+ ---------- + ---------- + ---------- + ---------- + -------- +
| Date       | EAN00001   | EAN00002   | EAN00003   | Total    |
+ ---------- + ---------- + ---------- + ---------- + -------- +
| 01/01/2020 | 10         | 10         | 10         | 30       |
| 01/02/2020 | 7          | 7          | 7          | 21       |
| 01/03/2020 | 6          | -3         | 7          | 13       |
| 01/04/2020 | 6          | 1          | 7          | 13       |
+ ---------- + ---------- + ---------- + ---------- + -------- +


=================================
Valuation of your solution
=================================
1 - A solution that works
2 - Naming of elements
3 - Non-repetition
4 - Use of adequate data structures
5 - Unit tests
6 - Respect of SOLID principles
