/* /Views/Coins */
function validateCoinValue() {
    var coinValueInput = document.getElementById('Value');
    var coinValue = parseInt(coinValueInput.value);
    var validationMessageSpan = document.getElementById('coinValueValidationMessage');

    if (![1, 2, 5, 10].includes(coinValue)) {
        validationMessageSpan.textContent = 'Coin value must be 1, 2, 5, or 10';
        coinValueInput.value = '';
    } else {
        validationMessageSpan.textContent = '';
    }
}


/*  /Views/Machine/Index  */
document.addEventListener("DOMContentLoaded", function () {
            var coinButtons = document.querySelectorAll(".coin-button");
            var buyButtons = document.querySelectorAll(".buy-btn");
            var returnChangeBtn = document.getElementById("return-change-btn");
            var currentAmountDisplay = document.getElementById("current-amount");
            var currentAmountHiddenInput = document.getElementById("hidden-current-amount");

            // Обработчик для добавления монет
            coinButtons.forEach(function (button) {
                button.addEventListener("click", function () {
                    addCoin(parseFloat(button.textContent)); 
                });
            });

            // Обработчик для покупки напитков
            buyButtons.forEach(function (button) {
                button.addEventListener("click", function () {
                    buyDrink(button);
                });
            });

            // Обработчик для возврата сдачи
            returnChangeBtn.addEventListener("click", function () {
                returnChange();
            });

            function addCoin(coinValue) {
                var formData = new FormData();
                formData.append("coinValue", coinValue);

                fetch("/Machine/UpdateCoinValue", {
                    method: "POST",
                    body: formData
                })
                    .then(handleResponse)
                    .then(function (data) {
                        if (data.success) {
                            var amount = parseInt(currentAmountHiddenInput.value) + parseInt(coinValue);
                            currentAmountDisplay.textContent = "Current amount: " + amount;
                            currentAmountHiddenInput.value = amount;
                            updateCoinInfo(coinValue);
                        } else {
                            alert(data.message);
                        }
                    })
                    .catch(handleError);
            }

            function buyDrink(button) {
                var drinkId = button.dataset.drinkId;
                var drinkPrice = parseFloat(button.dataset.drinkPrice);

                if (parseFloat(currentAmountHiddenInput.value) >= drinkPrice) {
                    fetch("/Machine/BuyDrink", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify({
                            drinkId: drinkId,
                            drinkPrice: drinkPrice,
                            currentAmount: parseFloat(currentAmountHiddenInput.value)
                        })
                    })
                        .then(handleResponse)
                        .then(function (data) {
                            alert(data.message);
                            if (data.success){
                                updateDrinkQuantity(drinkId);
                            updateCurrentAmount(data.currentAmount);
                            }
                            
                        })
                        .catch(handleError);
                } else {
                    alert("Not enough money to buy this drink!");
                }
            }


            function returnChange() {
    fetch("/Machine/GetCoinQuantities")
        .then(response => response.json())
        .then(function (data) {
            if (data.success) {

                var coinQuantities = data.coinQuantities;
                var currentAmount = parseFloat(currentAmountHiddenInput.value);
                var curAm = currentAmount;

                var tenCoins = Math.min(Math.floor(currentAmount / 10), coinQuantities.tenCoins);
                currentAmount -= tenCoins * 10;
                var fiveCoins = Math.min(Math.floor(currentAmount / 5), coinQuantities.fiveCoins);
                currentAmount -= fiveCoins * 5;
                var twoCoins = Math.min(Math.floor(currentAmount / 2), coinQuantities.twoCoins);
                currentAmount -= twoCoins * 2;
                var oneCoins = Math.min(currentAmount, coinQuantities.oneCoins);

                // Проверяем, хватает ли монет для сдачи
                if (curAm == tenCoins * 10 + fiveCoins * 5 + twoCoins * 2 + oneCoins) {
                    fetch("/Machine/ReturnChange", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify({
                            tenCoins: tenCoins,
                            fiveCoins: fiveCoins,
                            twoCoins: twoCoins,
                            oneCoins: oneCoins
                        })
                    })
                        .then(handleResponse)
                        .then(function (data) {
                            alert(data.message);
                            if (data.success) {
                                updateCurrentAmount(0);
                                location.reload();
                            }
                        })
                        .catch(handleError);
                } else {
                    alert("Not enough coins for change.");
                }
            } else {
                alert("Failed to get coin quantities.");
            }
        })
        .catch(handleError);
}




            function updateCoinInfo(coinValue) {
                var coinInfoId = "coin-quantity-" + coinValue;
                var coinInfoElement = document.getElementById(coinInfoId);
                var coinInfoText = coinInfoElement.textContent.split(" ");
                var currentCoinQuantity = parseInt(coinInfoText[coinInfoText.length - 2]);
                currentCoinQuantity++;
                coinInfoElement.textContent = coinValue + " ₽ - " + currentCoinQuantity + " pcs.";
            }

            function updateDrinkQuantity(drinkId) {
                var drinkQuantityField = document.getElementById("drink-quantity-" + drinkId);
                var drinkQuantityDisplay = document.querySelector(`#current-drink-quantity-${drinkId}`);
                var currentDrinkQuantity = parseInt(drinkQuantityField.value);
                currentDrinkQuantity--;
                drinkQuantityField.value = currentDrinkQuantity;
                drinkQuantityDisplay.textContent = `Quantity: ${currentDrinkQuantity} pcs.`;
            }

            function updateCurrentAmount(amount) {
                currentAmountDisplay.textContent = "Current amount: " + amount;
                currentAmountHiddenInput.value = amount;
            }

            function handleResponse(response) {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            }

            function handleError(error) {
                console.error("There was an error!", error);
                alert("Error: " + error.message);
            }
        });


/* /Drinks/Index */
document.getElementById('jsonFileInput').addEventListener('change', function () {
        var file = this.files[0];
        var formData = new FormData();
        formData.append('file', file);

        fetch("/Drinks/ImportDrinks", {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    alert('Failed to upload JSON file.');
                    throw new Error('Import failed');
                }
                alert('JSON file uploaded successfully.'); 
                location.reload(); 
            })
            .catch(error => {
                console.error(error); 
            });
    });
