function rand() {
    const array = new Uint32Array(1);
    window.crypto.getRandomValues(array);
    return Math.floor(array[0] / (0xFFFFFFFF + 1) * 10000000);
}

module.exports = {
	rand
}
