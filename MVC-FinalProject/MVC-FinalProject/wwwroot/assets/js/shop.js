"use strict"

//$(document).ready(function () {
//	const step = 10;
//	const minInput = $('input.slider-min');
//	const maxInput = $('input.slider-max');

//	function updateDisplay(minPrice, maxPrice) {
//		$('.current-min').text(minPrice + ' $');
//		$('.current-max').text(maxPrice + ' $');
//	}

//	function ajaxUpdate() {
//		let formData = $('#priceFilterForm').serialize();

//		const urlParams = new URLSearchParams(window.location.search);
//		['categoryName', 'brandName', 'colorName', 'tagName', 'sortType'].forEach(param => {
//			const val = urlParams.get(param) || '';
//			$(`#priceFilterForm input[name="${param}"]`).val(val);
//		});

//		$.ajax({
//			url: '@Url.Action("Index", "Shop")',
//			type: 'GET',
//			data: formData,
//			success: function (data) {
//				let parsed = new DOMParser().parseFromString(data, 'text/html');
//				let newProductsHtml = parsed.getElementById('productsContainer').innerHTML;
//				$('#productsContainer').html(newProductsHtml);

//				const minPrice = minInput.val();
//				const maxPrice = maxInput.val();

//				const urlParams = new URLSearchParams(window.location.search);
//				urlParams.set('minPrice', minPrice);
//				urlParams.set('maxPrice', maxPrice);

//				history.replaceState(null, '', window.location.pathname + '?' + urlParams.toString());
//			},
//			error: function () {
//			}
//		});
//	}

//	$('.slider').on('input', function () {
//		let minPrice = parseInt(minInput.val());
//		let maxPrice = parseInt(maxInput.val());

//		if ($(this).hasClass('slider-min')) {
//			if (minPrice >= maxPrice) {
//				minPrice = maxPrice - step;
//				if (minPrice < parseInt(minInput.attr('min'))) minPrice = parseInt(minInput.attr('min'));
//				minInput.val(minPrice);
//			}
//		} else if ($(this).hasClass('slider-max')) {
//			if (maxPrice <= minPrice) {
//				maxPrice = minPrice + step;
//				if (maxPrice > parseInt(maxInput.attr('max'))) maxPrice = parseInt(maxInput.attr('max'));
//				maxInput.val(maxPrice);
//			}
//		}

//		updateDisplay(minPrice, maxPrice);
//	});
//	$('.slider').on('change', function () {
//		ajaxUpdate();
//	});
//	updateDisplay(parseInt(minInput.val()) || 0, parseInt(maxInput.val()) || 400);
//});



//$(document).ready(function () {
//	function updateCurrentPriceDisplay() {
//		let min = $('.slider-min').val();
//		let max = $('.slider-max').val();

//		$('.current-min').text(min + ' $');
//		$('.current-max').text(max + ' $');
//	}
//	updateCurrentPriceDisplay();
//	$('.slider').on('input change', function () {
//		updateCurrentPriceDisplay();
//	});
//});