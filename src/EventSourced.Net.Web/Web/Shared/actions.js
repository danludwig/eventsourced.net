import { createAction } from 'redux-actions'

export const REDUX_INIT = '@@redux/INIT'
export const INITIALIZE_STATE = 'INITIALIZE_STATE'
export const SEND_WEBAPI = 'SEND_WEBAPI'

export function initialize() {
  return createAction(INITIALIZE_STATE)(window.___initialState___)
}
