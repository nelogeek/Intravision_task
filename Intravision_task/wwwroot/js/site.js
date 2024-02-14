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