import fetch from 'isomorphic-fetch'

export function submitToApi(context) {
  return dispatch => {
    if (typeof context.send === 'function')
      context.send(dispatch, context)

    const request = { }
    if (context.method) {
      request.method = context.method
    }
    if (context.formInput && request.method && request.method.toUpperCase() !== 'GET' && request.method.toUpperCase() !== 'HEAD') {
      request.body = JSON.stringify(context.formInput)
    }
    if (context.headers === undefined) {
      request.headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      }
    }
    if (context.credentials === undefined) {
      request.credentials = 'same-origin'
    }

    return fetch(`/api${context.url}`, request)
      .then(response => {
        // don't try to get json from a 404
        if (response.status === 404 && typeof context.fail === 'function') {
          return context.fail(dispatch, context, response, {
            _error: [{
              key: '_error',
              reason: 'notFound',
              message: 'An unknown error occurred.'
            }]
          })
        }

        const json = response.json()
        if (!response.ok && typeof context.fail === 'function') {
          return json.then(json => {
            let isJsonEmpty = true
            for (let x in json) {
              if (json.hasOwnProperty(x)) {
                isJsonEmpty = false
                break
              }
            }
            if (isJsonEmpty) {
              json = {
                _error: [{
                  key: '_error',
                  reason: 'badRequest',
                  message: 'An unknown error occurred.'
                }]
              }
            }
            return context.fail(dispatch, context, request, json)
          })
        }

        if (typeof context.done === 'function') {
          return json.then(json => {
            context.done(dispatch, context, response, json)
          })
        }
      })
  }
}
