const reducers = {
  sent: {
    next: (state, action) =>
      Object.assign({}, state, {
        apiCalls: [{
            sent: action.payload,
          },
          ...state.apiCalls,
        ],
      }),
    throw: (state, action) =>
      Object.assign({}, state, {
        apiCalls: [
          Object.assign({}, state.apiCalls[0], {
            fail: action.payload,
          }),
          ...state.apiCalls.slice(1)
        ],
      }),
  },
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
  over: (state, action) =>
    Object.assign({}, state, {
      apiCalls: [
        Object.assign({}, state.apiCalls[0], {
          over: { ...action.payload, },
        }),
        ...state.apiCalls.slice(1)
      ],
    }
  ),
}

const initialState = { apiCalls: [], }

export default {
  initialState,
  ...reducers,
}
