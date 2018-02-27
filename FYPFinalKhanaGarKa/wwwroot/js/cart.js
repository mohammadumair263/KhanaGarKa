$(document).ready(function () {

    // steps
    // 1. add class addItemBtn to plus btn of dish
    // 2. add class dish-price-int to dish price digit
    // 3. add id cart to the tbody tag of cart 
    // 4. now this jquery code will do all things 
    // 5. good luck to me and my team :D

    var prices = []; //arrys to store prices.
    var ID = 0; // Id assigned to each cart item.

    $("#cart").append( // appending empty cart view
        '<div class="cart-table ui-empty-cart" id="cart-empty-view" style="border-bottom:none">' +
        '<i class="fa fa-shopping-basket" aria-hidden="true"></i>' +
        '<p class="text-center">Add menu items into your basket</p>' +
        '</div>'
    );

    function sum_of_prices() {
        var subtotal_price = 0;
        for (var i = 0; i < prices.length; i++) {
            subtotal_price += prices[i];
        }
        return subtotal_price;

    }

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
        prices[ID] = cart_item_price; // adding individual price to array
        // $("#subtotal_price").text((parseInt($("#subtotal_price").text()) + parseInt(sum_of_prices())));// this line will calculate total for first time.
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
            var subtotal_price = $("#subtotal_price").text();
            var current_cart_item_price = $(this).siblings(".cart_item_price").text();
            // $("#subtotal_price").text((parseInt(subtotal_price) - parseInt(current_cart_item_price)));// this line will decrement subtotal after removing item.
        });

        $(".increse-val").prop('disabled', true);

        $(".plusBtn").off().on('click', function () {
            $(this).siblings(".increse-val").val(function (i, oldval) {
                oldval++;
                var Id = $(this).closest(".hobtr").attr("id");
                $(this).closest(".hobtr").find(".cart_item_price").text((prices[Id] * oldval));// this line will increase the price of single item.
                // $("#subtotal_price").text((parseInt($("#subtotal_price").text()) + parseInt(prices[Id])));// this line will increment subtotal after increasing quantity.
                $(this).closest(".hobtr").find(".qua").text(oldval);
                return oldval;
            });
        });

        $(".minusBtn").off().on('click', function () {
            $(this).siblings(".increse-val").val(function (i, oldval) {

                oldval--;
                if (oldval <= 1) {
                    oldval = 1;
                }
                var Id = $(this).closest(".hobtr").attr("id");
                $(this).closest(".hobtr").find(".cart_item_price").text((prices[Id] * oldval));// this line will decrease the price of current item.

                //var subtotal_price = $("#subtotal_price").text();
                //var curr_subtotal_price = (parseInt(subtotal_price) - parseInt(prices[Id]));

                //if (curr_subtotal_price <= 0)//this line prevent from decrementing to 0 if only one item
                //    curr_subtotal_price = subtotal_price;

                //$("#subtotal_price").text(curr_subtotal_price);// this line will decrement subtotal after decreasing quantity.
                $(this).closest(".hobtr").find(".qua").text(oldval);
                return oldval;
            });
        });

        ID++;
    });

});