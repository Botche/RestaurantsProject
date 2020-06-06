function createDomElement(type, textContent, classList, id, href) {
    const element = document.createElement(type);

    element.textContent = textContent;

    if (href) {
        element.href = href;
    }

    if (classList) {
        element.classList.add(...classList);
    }

    if (id) {
        element.setAttribute('id', id);
    }

    return element;
}

function createDomElementWithValue(type, value, classList, id, required, hidden, inputType, name) {
    const element = document.createElement(type);

    element.value = value;
    element.required = required;
    element.name = name;
    element.hidden = hidden;

    if (inputType) {
        element.type = inputType;
    }

    if (classList) {
        element.classList.add(...classList);
    }

    if (id) {
        element.setAttribute('id', id);
    }

    return element;
}

export {
    createDomElement,
    createDomElementWithValue,
}