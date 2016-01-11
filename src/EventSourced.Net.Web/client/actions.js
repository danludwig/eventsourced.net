import fetch from 'isomorphic-fetch'

export const INITIALIZE_STATE = 'INITIALIZE_STATE'
function initializeState() {
  return {
    type: INITIALIZE_STATE
  }
}

export const RECEIVE_INITIAL_STATE = 'RECEIVE_INITIAL_STATE'
function receiveInitialState(state) {
  return {
    type: RECEIVE_INITIAL_STATE,
    state
  }
}

export const FAIL_INITIAL_STATE = 'FAIL_INITIAL_STATE'
function failInitialState(state) {
  return {
    type: FAIL_INITIAL_STATE
  }
}

export function requestInitialState() {
  return dispatch => {
    dispatch(initializeState)

    return fetch(`/api`, {
      headers: {
        'Accept': 'application/json',
        'X-Requested-With': 'isomorphic-fetch'
      },
      credentials: 'same-origin'
    })
      .then(response => {
        if (!response.ok)
          return dispatch(failInitialState())
        return response.json().then(json =>
          dispatch(receiveInitialState(json))
        )
      })
  }
}
