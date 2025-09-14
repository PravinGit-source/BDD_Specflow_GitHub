@Regression @new
Feature: EndToEndFlow_CSharp_RSA

  Scenario: EndToEndFlow_CSharp_RSA
    Given I am on the login page
    When I enter username and password 
    And I click the sign-in button
    Then I should see the product page
    When I add the following products to the cart:
      | Product        |
      | iphone X       |
      | Blackberry     |
      | Samsung Note 8 |
      | Nokia Edge     |
    And I proceed to checkout
    #Then the selected products should be in the checkout list
    When I enter ind in the country field
    When I confirm the purchase
    Then I should see a success message