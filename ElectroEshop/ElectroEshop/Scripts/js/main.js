(function($) {
    "use strict"

    /////////////////////////////////////////

	// subscribe newsletter

    $('.closeNewsletter').click(function () {
        $('#newsletterError').removeClass('show');
        $('#newsletterError').addClass('hide');
        $('#newsletterSuccess').removeClass('show');
        $('#newsletterSuccess').addClass('hide');
    });

    var newsletterEmail;

    $('#newsletter_btn').click(function () {
        newsletterEmail = $('#newsletter_email').val();
        registerNewsletter();
    });

    function registerNewsletter() {
        $.ajax({
            type: "GET",
            url: '/Home/RegisterNewsletter/)',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: {
                newsletterEmail: newsletterEmail
            },
            success: function (data) {
                $('#newsletterSuccess').addClass('show');
                //console.log(data.responseText);
            },
            failure: function (response) {
                $('#newsletterError').addClass('show');
                //console.log(response.responseText);
            },
            error: function (response) {
                $('#newsletterError').addClass('show');
                //console.log(response.responseText);
            }
        });
    }

    /////////////////////////////////////////

	// filter products

    var changedPage = 6;
    var changedSorting = 6;
    var changedMinPrice = 1;
    var changedMaxPrice = 9999;
    var checkedCategories = "";
    var checkedBrands = "";

    $('#sortSelectList').change(function () {
        changedSorting = $(this).val();
        console.log($(this).val());
        callAjaxFunction();
    });
    $('#pageSelectList').change(function () {
        changedPage = $(this).val();
        console.log(changedPage);
        callAjaxFunction();
    });
    $(".price-min").change(function () {
        changedMinPrice = parseInt($('#price-min').val());
        console.log("I am id " + $('#price-min').val());
        callAjaxFunction();
    });
    $(".price-max").change(function () {
        changedMaxPrice = parseInt($('#price-max').val());
        console.log(changedMaxPrice);
        callAjaxFunction();
    });
    $("#input-category").change(function () {
        callAjaxFunction();
    });
    $("#input-brand").change(function () {
        callAjaxFunction();
    });
    function checkedCategoriesFunction() {
        $("#input-category input[type = 'checkbox']").each(function () {
            var c = $(this).is(":checked");
            console.log("I am HERE");
            console.log($(this));
            if (c) {
                var checkedId = $(this).attr("id");
                if (!checkedCategories.includes(checkedId)) {
                    (checkedCategories == "" ? checkedCategories += checkedId : checkedCategories += "+" + checkedId);  //console.log(checkedCategories);
                }
            }
        });
        $("#input-brand input[type = 'checkbox']").each(function () {
            var c = $(this).is(":checked");
            //console.log("** ** ** **");
            //console.log($(this));
            if (c) {
                var checkedId = $(this).attr("id");
                if (!(checkedBrands.includes(checkedId))) {
                    (checkedBrands == "" ? checkedBrands += checkedId : checkedBrands += "+" + checkedId);              //console.log(checkedBrands);
                }
            }
        });
    }

    function callAjaxFunction() {
        checkedCategoriesFunction();
        $.ajax({
            type: "Get",
            url: '/Home/filterAjax/)',
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            data: {
                categoriesString: checkedCategories,
                brandsString: checkedBrands,
                changedMinPrice: changedMinPrice,
                changedMaxPrice: changedMaxPrice,
                changedSorting: changedSorting,
                changedPage: changedPage,
            },
            success: function (data) {
                //console.log(data);

                $('#changeWithAjax').empty();
                $('#changeWithAjax').html(data);
                checkedCategories = "";
                checkedBrands = "";
            },
            failure: function (response) {
                console.log(response.responseText);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }

    /////////////////////////////////////////

	// Mobile Nav toggle
	$('.menu-toggle > a').on('click', function (e) {
		e.preventDefault();
		$('#responsive-nav').toggleClass('active');
	})

	// Fix cart dropdown from closing
	$('.cart-dropdown').on('click', function (e) {
		e.stopPropagation();
	});

	/////////////////////////////////////////

	// Products Slick
	$('.products-slick').each(function() {
		var $this = $(this),
				$nav = $this.attr('data-nav');

		$this.slick({
			slidesToShow: 4,
			slidesToScroll: 1,
			autoplay: true,
			infinite: true,
			speed: 300,
			dots: false,
			arrows: true,
			appendArrows: $nav ? $nav : false,
			responsive: [{
	        breakpoint: 991,
	        settings: {
	          slidesToShow: 2,
	          slidesToScroll: 1,
	        }
	      },
	      {
	        breakpoint: 480,
	        settings: {
	          slidesToShow: 1,
	          slidesToScroll: 1,
	        }
	      },
	    ]
		});
	});

	// Products Widget Slick
	$('.products-widget-slick').each(function() {
		var $this = $(this),
				$nav = $this.attr('data-nav');

		$this.slick({
			infinite: true,
			autoplay: true,
			speed: 300,
			dots: false,
			arrows: true,
			appendArrows: $nav ? $nav : false,
		});
	});

	/////////////////////////////////////////

	// Product Main img Slick
	$('#product-main-img').slick({
    infinite: true,
    speed: 300,
    dots: false,
    arrows: true,
    fade: true,
    asNavFor: '#product-imgs',
  });

	// Product imgs Slick
  $('#product-imgs').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    arrows: true,
    centerMode: true,
    focusOnSelect: true,
		centerPadding: 0,
		vertical: true,
    asNavFor: '#product-main-img',
		responsive: [{
        breakpoint: 991,
        settings: {
					vertical: false,
					arrows: false,
					dots: true,
        }
      },
    ]
  });

	// Product img zoom
	var zoomMainProduct = document.getElementById('product-main-img');
	if (zoomMainProduct) {
		$('#product-main-img .product-preview').zoom();
	}

	/////////////////////////////////////////

	// Input number
	$('.input-number').each(function() {
		var $this = $(this),
		$input = $this.find('input[type="number"]'),
		up = $this.find('.qty-up'),
		down = $this.find('.qty-down');

		down.on('click', function () {
			var value = parseInt($input.val()) - 1;
			value = value < 1 ? 1 : value;
			$input.val(value);
			$input.change();
			updatePriceSlider($this , value)
		})

		up.on('click', function () {
			var value = parseInt($input.val()) + 1;
			$input.val(value);
			$input.change();
			updatePriceSlider($this , value)
		})
	});

	var priceInputMax = document.getElementById('price-max'),
			priceInputMin = document.getElementById('price-min');

    if (priceInputMax) {
        priceInputMax.addEventListener('change', function () {
            updatePriceSlider($(this).parent(), this.value)
        });
    }
    if (priceInputMin) {
        priceInputMin.addEventListener('change', function () {
            updatePriceSlider($(this).parent(), this.value)
        });
    }

	function updatePriceSlider(elem , value) {
        if (elem.hasClass('price-min')) {
            console.log('min ' + value)
			priceSlider.noUiSlider.set([value, null]);
        } else if (elem.hasClass('price-max')) {
            console.log('max ' + value)
			priceSlider.noUiSlider.set([null, value]);
		}
	}

	// Price Slider
	var priceSlider = document.getElementById('price-slider');
	if (priceSlider) {
		noUiSlider.create(priceSlider, {
            start: [1, 9999],
            connect: true,
			step: 1,
			range: {
				'min': 1,
				'max': 9999
            }
		});

		priceSlider.noUiSlider.on('update', function( values, handle ) {
			var value = values[handle];
			handle ? priceInputMax.value = value : priceInputMin.value = value
		});
	}

})(jQuery);
