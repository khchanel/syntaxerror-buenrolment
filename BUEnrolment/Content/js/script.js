function moveItems(sourceID, destID) {
    var src = document.getElementById(sourceID);
    var dest = document.getElementById(destID);

    for (var count = 0; count < src.options.length; count++) {

        if (src.options[count].selected == true) {
            var option = src.options[count];

            var newOption = document.createElement("option");
            newOption.value = option.value;
            newOption.text = option.text;
            newOption.selected = true;
            try {
                dest.add(newOption, null); //Standard
                src.remove(count, null);
            } catch (error) {
                dest.add(newOption); // IE only
                src.remove(count);
            }
            count--;
        }
    }
}