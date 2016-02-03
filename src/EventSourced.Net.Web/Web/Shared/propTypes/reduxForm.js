const formName = {
  formName: React.PropTypes.string.isRequired,
}

const formKey = {
  formKey: React.PropTypes.string,
}

const handleSubmit = {
  handleSubmit: React.PropTypes.func.isRequired,
}

const submitting = {
  submitting: React.PropTypes.bool.isRequired,
}

const submitFailed = {
  submitFailed: React.PropTypes.bool.isRequired,
}

const error = {
  error: React.PropTypes.string,
}

const asyncValidating = {
  asyncValidating: React.PropTypes.oneOfType([
    React.PropTypes.string,
    React.PropTypes.bool,
  ]).isRequired,
}

const dispatch = {
  dispatch: React.PropTypes.func.isRequired,
}

const field = React.PropTypes.shape({
  name: React.PropTypes.string.isRequired,
  value: React.PropTypes.string,
})

export default {
  formName,
  formKey,
  handleSubmit,
  submitting,
  submitFailed,
  error,
  asyncValidating,
  dispatch,
  field,
}
