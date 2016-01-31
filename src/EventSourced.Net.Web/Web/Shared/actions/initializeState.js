import { createAction } from 'redux-actions'

export const INITIALIZE_STATE = 'INITIALIZE_STATE'

export default () => (
  createAction(INITIALIZE_STATE)(window.___initialState___)
)
