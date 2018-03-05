$(document).ready(function () {

    // steps
    // 1. add class addItemBtn to plus btn of dish
    // 2. add class dish-price-int to dish price digit
    // 3. add id cart to the tbody tag of cart 
    // 4. now this jquery code will do all things

    var prices = []; //arrys to store prices.
    var ID = 0; // Id assigned to each cart item.
    var names = [];// dish names will stored in this array
    var quantities = [];// quantities to store in this array
    var subtotal_price = 0;
    var delivery_fee_price = 10;
    var full_total_price = 0;

    $("#cart").append( // appending empty cart view
        '<div class="cart-table ui-empty-cart" id="cart-empty-view" style="border-bottom:none">' +
        '<i class="fa fa-shopping-basket" aria-hidden="true"></i>' +
        '<p class="text-center">Add menu items into your basket</p>' +
        '</div>'
    );

    function get_names() {
        var name = [];
        var j = 0;
        for (var i = 0; i < names.length; i++) {
            if (names[i] != "") {
                name[j] = names[i];
                j++;
            }
        }
        return name;
    }
    function get_prices() {
        var price = [];
        var j = 0;
        for (var i = 0; i < prices.length; i++) {
            if (prices[i] != 0) {
                price[j] = prices[i];
                j++;
            }
        }
        return price;
    }
    function get_quantities() {
        var qua = [];
        var j = 0;
        for (var i = 0; i < quantities.length; i++) {
            if (quantities[i] != 0) {
                qua[j] = quantities[i];
                j++;
            }
        }
        return qua;
    }

    $(".addItemBtn").off().on('click', function () {
        var cart_item_name = $(this).closest(".dish-content").data("name");
        var cart_item_price = $(this).closest(".dish-content").data("price");

        $("#cart").find("#cart-empty-view").remove(); //removing empty cart view
        $("#cart").append(
            '<tr class="hobtr" data-id="' + ID + '">' +
            '<td class="cross-td custom-spinner">' +
            '<div class="input-group spinner">' +
            '<button class="btn btn-default cust-plus plusBtn" type="button"><i class="fa fa-plus"></i></button>' +
            '<input type="text" class="form-control increse-val" value="1">' +
            '<button class="btn btn-default cust-plus minusBtn" type="button"><i class="fa fa-minus"></i></button>' +
            '</div>' +
            '<span class="qnt-idn"><span class="qua">1</span> x</span>' +
            '</td>' +
            '<td class="itme-td">' + cart_item_name + '</td>' +
            '<td class="amont-td"><i class="fa fa-times-circle-o closeBtn"></i> <span class="main-price cart_item_price">' + cart_item_price + '</span></td>' +
            ' </tr>'
        );
        prices[ID] = cart_item_price; // adding price of item that is added to array.
        names[ID] = cart_item_name; //adding dish name to the array.
        quantities[ID] = 1; //adding quantities to the array.
        // this line will calculate total every time when new item is inserted in the cart.{
        subtotal_price = subtotal_price + parseInt(prices[ID]);
        $("#subtotal_price").text(subtotal_price);
        full_total_price = subtotal_price + delivery_fee_price; // setting total price.
        $("#full_total_price").text(full_total_price);
        $("#total-cart-amount").text(full_total_price);//setting the total amout of mobile cart
        // end }
        $(".closeBtn").off().on('click', function () {
            var Id = $(this).closest(".hobtr").data("id");
            $(this).closest(".hobtr").remove();
            if ($("#cart").children().length <= 0) {
                $("#cart").append(
                    '<div class="cart-table ui-empty-cart" id="cart-empty-view" style="border-bottom:none">' +
                    '<i class="fa fa-shopping-basket" aria-hidden="true"></i>' +
                    '<p class="text-center">Add menu items into your basket</p>' +
                    '</div>'
                );
            }
            //finding what is current value of item.
            var current_price = parseInt(prices[Id]) * parseInt($(this).closest(".hobtr").find(".increse-val").val());
            subtotal_price = subtotal_price - current_price; // subtracting deleted item value from subtotal.
            $("#subtotal_price").text(subtotal_price);
            full_total_price = subtotal_price + delivery_fee_price; // setting total price.
            if (full_total_price == 10)//checking and setting the total price to zero when cart is empty
                full_total_price = 0;
            $("#full_total_price").text(full_total_price);
            $("#total-cart-amount").text(full_total_price);//setting the total amout of mobile cart
            prices[Id] = 0; // setting the price to 0 which is removed from cart.
            names[Id] = ""; // setting the name to empty string which is removed from the cart.
            quantities[Id] = 0; // setting the quantities to 0 which is removed from cart
        });

        $(".increse-val").prop('disabled', true);

        $(".plusBtn").off().on('click', function () {
            $(this).siblings(".increse-val").val(function (i, oldval) {
                oldval++;
                var Id = $(this).closest(".hobtr").data("id");
                $(this).closest(".hobtr").find(".cart_item_price").text((prices[Id] * oldval));// this line will increase the price of single item.

                // getting the price from prices array and then setting it
                subtotal_price = subtotal_price + parseInt(prices[$(this).closest(".hobtr").data("id")]);
                $("#subtotal_price").text(subtotal_price);
                full_total_price = subtotal_price + delivery_fee_price; // setting total price.
                $("#full_total_price").text(full_total_price);
                $("#total-cart-amount").text(full_total_price);//setting the total amout of mobile cart

                $(this).closest(".hobtr").find(".qua").text(oldval);
                quantities[Id] = oldval;// adding the value to array.
                return oldval;
            });
        });

        $(".minusBtn").off().on('click', function () {
            $(this).siblings(".increse-val").val(function (i, oldval) {
                var Id = $(this).closest(".hobtr").data("id");
                if (oldval > 1) // this chek the quantity of item.
                    subtotal_price = subtotal_price - parseInt(prices[$(this).closest(".hobtr").data("id")]);
                $("#subtotal_price").text(subtotal_price);
                full_total_price = subtotal_price + delivery_fee_price; // setting total price.
                $("#full_total_price").text(full_total_price);
                $("#total-cart-amount").text(full_total_price);//setting the total amout of mobile cart
                oldval--;
                if (oldval <= 1) {
                    oldval = 1;
                }
                
                $(this).closest(".hobtr").find(".cart_item_price").text((prices[Id] * oldval));// this line will decrease the price of current item.
                $(this).closest(".hobtr").find(".qua").text(oldval);
                quantities[Id] = oldval;// adding the value to array.
                return oldval;
            });
        });

        ID++;

        
    });
    $(document).ready(function () {
        $('#pro').click(function () {
            var datasource = {
                'Price': prices,
                'Name': names,
                'Quantity': quantities,
                'Subtotal': subtotal_price,
                'Total': full_total_price
             };


            $.ajax({
                url: "/Order/PostJson",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(datasource),
                success: function (data) {
                    if (data.state == 0) {
                        document.location.reload();
                    }
                }
            });
        });
    });
    


});