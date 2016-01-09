import $ from 'jquery';
import {markdown} from 'markdown';

console.log(markdown);

export function setFocus(formId, fieldName) {
  $("#" + formId).find("[name=" + fieldName + "]").focus();
}

export function serializeFormData($form) {
  var data = {};
  $form.serializeArray().map(function (x) { data[x.name] = x.value; });
  return data;
}

export function disableForm($form) {
  $form.find("input, button").attr("disabled", "disabled");
}

export function enableForm($form) {
  $form.find("input, button").removeAttr("disabled");
}

export function getCorrelationSocket(xhr) {
  var correlationUrl = xhr.getResponseHeader("x-correlation-socket");
  var socket = new WebSocket(correlationUrl);
  return socket;
}

export function parseMessageData(message) {
  var messageData = { type: "unknown" };
  try {
    messageData = $.parseJSON(message.data);
  } catch (ex) { }
  return messageData;
}

export function setLocation(xhr) {
  window.location = xhr.getResponseHeader("location");
}

export function handleValidationErrors(xhr, $form, messages) {
  if (xhr.status === 400 && xhr.responseJSON) {
    var modelState = xhr.responseJSON;
    for (var fieldName in modelState) {
      if (!modelState.hasOwnProperty(fieldName)) continue;
      var fieldErrors = modelState[fieldName];
      if (!fieldErrors.length) continue;
      for (var i = 0; i < fieldErrors.length; i++) {
        var message = getValidationMessage(fieldErrors[i], messages);
        addValidationError($form, message);
      }
    }
  }
}

export function getValidationMessage(item, messages) {
  var key = camelizeHack(item.key);
  var reason = camelizeHack(item.reason);
  if (messages[key] && messages[key][reason]) {
    var message = messages[key][reason] || item.message;
    message = message.replace("{attemptedValue}", item.value);
    if (item.data) {
      for (var fieldName in item.data) {
        if (!item.data.hasOwnProperty(fieldName)) continue;
        message = message.replace("{" + fieldName + "}", item.data[fieldName]);
      }
    }
    return markdown.toHTML(message);
  }
  return item.message || "An unexpected error occurred.";
}

export function addValidationError($form, message) {
  var $ul = $form.find(".form-errors ul");
  $ul.append($("<li></li>").html(message));
}

export function clearValidationErrors($form) {
  var $ul = $form.find(".form-errors ul");
  $ul.empty();
}

export function camelizeHack(text) {
  return text.substr(0, 1).toLowerCase() + text.substr(1);
}
