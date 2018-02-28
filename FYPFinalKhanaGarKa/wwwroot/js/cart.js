$(document).ready(function () {

    // steps
    // 1. add class addItemBtn to plus btn of dish
    // 2. add class dish-price-int to dish price digit
    // 3. add id cart to the tbody tag of cart 
    // 4. now this jquery code will do all things 
    // 5. good luck to me and my team :D
    // 6. hope we will get A+ in our project

    var prices = []; //arrys to store prices.
    var ID = 0; // Id assigned to each cart item.
    var subtotal_price = 0;
    var delivery_fee_price = 10;
    var full_total_price = 0;

    $("#cart").append( // appending empty cart view
        '<div class="cart-table ui-empty-cart" id="cart-empty-view" style="border-bottom:none">' +
        '<i class="fa fa-shopping-basket" aria-hidden="true"></i>' +
        '<p class="text-center">Add menu items into your basket</p>' +
        '</div>'
    );

    $(".addItemBtn").off().on('click', function () {
        var cart_item_name = $(this).closest(".dish-content").find('.dish-name').text();
        var cart_item_price = $(this).closest(".dish-content").find('.dish-price-int').text();

        $("#cart").find("#cart-empty-view").remove(); //removing empty cart view
        $("#cart").append(
            '<tr class="hobtr" id="' + ID + '">' +
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
        // this line will calculate total every time when new item is inserted in the cart.{
        subtotal_price = subtotal_price + parseInt(prices[ID]);
        $("#subtotal_price").text(subtotal_price);
        full_total_price = subtotal_price + delivery_fee_price; // setting total price.
        $("#full_total_price").text(full_total_price);
        $("#total-cart-amount").text(full_total_price);//setting the total amout of mobile cart
        // end }
        $(".closeBtn").off().on('click', function () {
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
            var current_price = parseInt(prices[$(this).closest(".hobtr").attr("id")]) * parseInt($(this).closest(".hobtr").find(".increse-val").val());
            subtotal_price = subtotal_price - current_price; // subtracting deleted item value from subtotal.
            $("#subtotal_price").text(subtotal_price);
            full_total_price = subtotal_price + delivery_fee_price; // setting total price.
            $("#full_total_price").text(full_total_price);
            $("#total-cart-amount").text(full_total_price);//setting the total amout of mobile cart
        });

        $(".increse-val").prop('disabled', true);

        $(".plusBtn").off().on('click', function () {
            $(this).siblings(".increse-val").val(function (i, oldval) {
                oldval++;
                var Id = $(this).closest(".hobtr").attr("id");
                $(this).closest(".hobtr").find(".cart_item_price").text((prices[Id] * oldval));// this line will increase the price of single item.

                // getting the price from prices array and then setting it
                subtotal_price = subtotal_price + parseInt(prices[$(this).closest(".hobtr").attr("id")]);
                $("#subtotal_price").text(subtotal_price);
                full_total_price = subtotal_price + delivery_fee_price; // setting total price.
                $("#full_total_price").text(full_total_price);
                $("#total-cart-amount").text(full_total_price);//setting the total amout of mobile cart

                $(this).closest(".hobtr").find(".qua").text(oldval);
                return oldval;
            });
        });

        $(".minusBtn").off().on('click', function () {
            $(this).siblings(".increse-val").val(function (i, oldval) {
                if (oldval > 1) // this chek the quantity of item.
                    subtotal_price = subtotal_price - parseInt(prices[$(this).closest(".hobtr").attr("id")]);
                $("#subtotal_price").text(subtotal_price);
                full_total_price = subtotal_price + delivery_fee_price; // setting total price.
                $("#full_total_price").text(full_total_price);
                $("#total-cart-amount").text(full_total_price);//setting the total amout of mobile cart
                oldval--;
                if (oldval <= 1) {
                    oldval = 1;
                }
                var Id = $(this).closest(".hobtr").attr("id");
                $(this).closest(".hobtr").find(".cart_item_price").text((prices[Id] * oldval));// this line will decrease the price of current item.
                $(this).closest(".hobtr").find(".qua").text(oldval);
                return oldval;
            });
        });

        ID++;
    });

});