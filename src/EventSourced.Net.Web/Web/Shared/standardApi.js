import CALL_API from 'redux-api-middleware/lib/CALL_API'
import { getJSON } from 'redux-api-middleware/lib/util'

const createAction = config => ({
  [CALL_API]: {
    types: [
      {
        type: config.types[0], // send
        payload: (action, state) => {
          const { method, endpoint, body } = action[CALL_API]
          const formInput = JSON.parse(body)
          return { method, endpoint, formInput, }
        },
      },
      {
        type: config.types[1], // done
        payload: (action, state, response) => {
          const { headers } = response
          return getJSON(response).then(data => ({
            data,
            headers: {
              location: headers.get('location'),
              correlationSocket: headers.get('x-correlation-socket'),
            },
          }))
        },
      },
      config.types[2], // fail
    ],
    method: config.method || 'GET',
    endpoint: config.endpoint,
    body: config.body ? JSON.stringify(config.body) : '{}',
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    },
    credentials: 'same-origin',
  }
})

const reducers = {
  sent: (state, action) =>
    Object.assign({}, state, {
      apiCalls: [{
          sent: action.payload,
        },
        ...state.apiCalls,
      ],
    }
  ),
  fail: (state, action) =>
    Object.assign({}, state, {
      apiCalls: [
        Object.assign({}, state.apiCalls[0], {
          fail: {
            status: action.payload.status,
            response: action.payload.response,
          },
        }),
        ...state.apiCalls.slice(1)
      ],
    }
  ),
  done: (state, action) =>
    Object.assign({}, state, {
      apiCalls: [
        Object.assign({}, state.apiCalls[0], {
          done: { ...action.payload, },
        }),
        ...state.apiCalls.slice(1)
      ],
    }
  ),
}

const initialState = { apiCalls: [], }

export default {
  createAction,
  reducers,
  initialState,
}
