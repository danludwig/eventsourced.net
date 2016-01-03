function setFocus(formId, fieldName) {
  $("#" + formId).find("[name=" + fieldName + "]").focus();
}
function serializeFormData($form) {
  var data = {};
  $form.serializeArray().map(function (x) { data[x.name] = x.value; });
  return data;
}
function disableForm($form) {
  $form.find("input, button").attr("disabled", "disabled");
}
function getCorrelationSocket(xhr) {
  var correlationUrl = xhr.getResponseHeader("x-correlation-socket");
  var socket = new WebSocket(correlationUrl);
  return socket;
}
function parseMessageData(message) {
  var messageData = { type: "unknown" };
  try {
    messageData = $.parseJSON(message.data);
  } catch (ex) { }
  return messageData;
}
function setLocation(xhr) {
  window.location = xhr.getResponseHeader("location");
}
function enableForm($form) {
  $form.find("input, button").removeAttr("disabled");
}
function handleValidationErrors(xhr, $form) {
  if (xhr.status === 400 && xhr.responseJSON) {
    var modelState = xhr.responseJSON;
    for (var fieldName in modelState) {
      if (!modelState.hasOwnProperty(fieldName)) continue;
      var fieldErrors = modelState[fieldName];
      if (!fieldErrors.length) continue;
      for (var i = 0; i < fieldErrors.length; i++) {
        var message = getValidationMessage(fieldErrors[i]);
        addValidationError($form, message);
      }
    }
  }
}
function addValidationError($form, message) {
  var $ul = $form.find(".form-errors ul");
  $ul.append($("<li></li>").text(message));
}
function clearValidationErrors($form) {
  var $ul = $form.find(".form-errors ul");
  $ul.empty();
}
function getValidationMessage(item) {
  var messages = window["validationMessages"];
  if (messages[item.key] && messages[item.key][camelizeHack(item.reason)]) {
    var message = messages[item.key][camelizeHack(item.reason)] || item.message;
    message = message.replace("{attemptedValue}", item.value);
    if (item.data) {
      for (var fieldName in item.data) {
        if (!item.data.hasOwnProperty(fieldName)) continue;
        message = message.replace("{" + fieldName + "}", item.data[fieldName]);
      }
    }
    return message;
  }
  return item.message || "An unexpected error occurred.";
}
function camelizeHack(text) {
  return text.substr(0, 1).toLowerCase() + text.substr(1);
}
