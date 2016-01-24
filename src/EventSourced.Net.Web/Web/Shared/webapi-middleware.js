import fetch from 'isomorphic-fetch'
import { SEND_WEBAPI } from './actions'

export default function promiseMiddleware() {
  return (next) => (action) => {
    const { type, payload, ...rest } = action
    if (type !== SEND_WEBAPI) {
      return next(action)
    }

    if (typeof payload.send === 'function')
      next(payload.send())

    return dispatch => {
      const request = { }
      if (payload.method) {
        request.method = payload.method
      }
      if (payload.formInput && request.method && request.method.toUpperCase() !== 'GET' && request.method.toUpperCase() !== 'HEAD') {
        request.body = JSON.stringify(payload.formInput)
      }
      if (payload.headers === undefined) {
        request.headers = {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        }
      }
      if (payload.credentials === undefined) {
        request.credentials = 'same-origin'
      }

      return fetch(`/api${payload.url}`, request)
      .then(response => {
        // don't try to get json from a 404
        if (response.status === 404 && typeof payload.fail === 'function') {
          return payload.fail(dispatch, payload, response, {
            _error: [{
              key: '_error',
              reason: 'notFound',
              message: 'An unknown error occurred.'
            }]
          })
        }

        const json = response.json()
        if (!response.ok && typeof payload.fail === 'function') {
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
            return payload.fail(dispatch, payload, request, json)
          })
        }

        if (typeof payload.done === 'function') {
          return json.then(json => {
            payload.done(dispatch, payload, response, json)
          })
        }
      })
    }
  }
}
